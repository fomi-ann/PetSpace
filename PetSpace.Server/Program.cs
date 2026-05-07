using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetSpace.ApplicationServices.Services;
using PetSpace.Core.Domain;
using PetSpace.Core.ServiceInterface;
using PetSpace.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PetSpaceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, UserRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<PetSpaceDbContext>()
.AddDefaultTokenProviders();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddScoped<IPetSpaceServices, PetSpaceServices>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;


    var context = services.GetRequiredService<PetSpaceDbContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<UserRole>>();

    await context.Database.EnsureDeletedAsync();
    await context.Database.EnsureCreatedAsync();

    await SeedRoles(roleManager);
    await SeedAppointmentStatuses(context);
    await SeedMockData(userManager, context);
}

app.UseDefaultFiles();
app.MapStaticAssets();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

static async Task SeedRoles(RoleManager<UserRole> roleManager)
{
    string[] roleNames = { "Admin", "Clinic", "Vet", "PetOwner" };

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);

        if (!roleExist)
        {
            await roleManager.CreateAsync(new UserRole { Name = roleName });
        }
    }
}

static async Task SeedAppointmentStatuses(PetSpaceDbContext context)
{
    if (!context.AppointmentStatusCodes.Any())
    {
        context.AppointmentStatusCodes.AddRange(
            new AppointmentStatusCode { AppStatusCode = "Pending" },
            new AppointmentStatusCode { AppStatusCode = "Approved" },
            new AppointmentStatusCode { AppStatusCode = "Rejected" },
            new AppointmentStatusCode { AppStatusCode = "Completed" }
        );

        await context.SaveChangesAsync();
    }
}

static async Task SeedMockData(
    UserManager<User> userManager,
    PetSpaceDbContext context)
{
    const string password = "Test123!";

    if (await context.Users.AnyAsync(u => u.Email == "owner1@test.ee"))
        return;

    async Task<User> CreateUser(string email, string firstName, string lastName, string role)
    {
        var user = new User
        {
            UserName = email,
            Email = email,
            UserFirstName = firstName,
            UserLastName = lastName,
            PhoneNumber = "555-0000"
        };

        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, role);

        return user;
    }

    var clinicUser1 = await CreateUser("clinic1@test.ee", "Happy", "Paws", "Clinic");
    var clinicUser2 = await CreateUser("clinic2@test.ee", "Crazy", "Pets", "Clinic");

    var clinic1 = new Clinic
    {
        ClinicId = Guid.NewGuid(),
        UserId = clinicUser1.Id,
        ClinicName = "Happy Paws Clinic",
        ClinicAddress = "Tallinn, Estonia",
        ClinicPhone = "555-1111"
    };

    var clinic2 = new Clinic
    {
        ClinicId = Guid.NewGuid(),
        UserId = clinicUser2.Id,
        ClinicName = "Crazy Pets Hospital",
        ClinicAddress = "Tartu, Estonia",
        ClinicPhone = "555-2222"
    };

    context.Clinics.AddRange(clinic1, clinic2);

    var vetUser1 = await CreateUser("ace@test.ee", "Ace", "Ventura", "Vet");
    var vetUser2 = await CreateUser("dolittle@test.ee", "Doctor", "Dolittle", "Vet");
    var vetUser3 = await CreateUser("house@test.ee", "Gregory", "House", "Vet");
    var vetUser4 = await CreateUser("watson@test.ee", "Dr.", "Watson", "Vet");

    var vet1 = new Vet
    {
        VetId = Guid.NewGuid(),
        UserId = vetUser1.Id,
        VetSpecialization = "All animals",
        VetLicence = "ACE-001",
        ClinicId = clinic1.ClinicId,
        IsVerified = true,
        VerifiedAt = DateTime.UtcNow
    };

    var vet2 = new Vet
    {
        VetId = Guid.NewGuid(),
        UserId = vetUser2.Id,
        VetSpecialization = "Exotic animals",
        VetLicence = "DOL-002",
        ClinicId = clinic1.ClinicId,
        IsVerified = true,
        VerifiedAt = DateTime.UtcNow
    };

    var vet3 = new Vet
    {
        VetId = Guid.NewGuid(),
        UserId = vetUser3.Id,
        VetSpecialization = "Diagnostics",
        VetLicence = "HOU-003",
        ClinicId = clinic2.ClinicId,
        IsVerified = true,
        VerifiedAt = DateTime.UtcNow
    };

    var vet4 = new Vet
    {
        VetId = Guid.NewGuid(),
        UserId = vetUser4.Id,
        VetSpecialization = "Small animals",
        VetLicence = "WAT-004",
        ClinicId = clinic1.ClinicId,
        IsVerified = false,
        VerifiedAt = null
    };

    context.Vets.AddRange(vet1, vet2, vet3, vet4);

    var owner1 = await CreateUser("owner1@test.ee", "John", "Wick", "PetOwner");
    var owner2 = await CreateUser("owner2@test.ee", "Tony", "Stark", "PetOwner");
    var owner3 = await CreateUser("owner3@test.ee", "Hermione", "Granger", "PetOwner");

    var pet1 = new Pet
    {
        PetId = Guid.NewGuid(),
        PetName = "Mr. Snuggles",
        PetSpecies = "Cat",
        PetBreed = "British Shorthair",
        PetGender = "Male",
        PetBirthDate = DateTime.UtcNow.AddYears(-3).AddMonths(-2),
        PetMicrochipNr = "CHIP-001",
        PetWeight = 5.4m,
        OwnerId = owner1.Id
    };

    var pet2 = new Pet
    {
        PetId = Guid.NewGuid(),
        PetName = "Bark Twain",
        PetSpecies = "Dog",
        PetBreed = "Golden Retriever",
        PetGender = "Male",
        PetBirthDate = DateTime.UtcNow.AddYears(-5).AddMonths(-6),
        PetMicrochipNr = "CHIP-002",
        PetWeight = 28.7m,
        OwnerId = owner2.Id
    };

    var pet3 = new Pet
    {
        PetId = Guid.NewGuid(),
        PetName = "Lord Voldemutt",
        PetSpecies = "Dog",
        PetBreed = "Doberman",
        PetGender = "Female",
        PetBirthDate = DateTime.UtcNow.AddYears(-2).AddMonths(-1),
        PetMicrochipNr = "CHIP-003",
        PetWeight = 31.2m,
        OwnerId = owner3.Id
    };

    context.Pets.AddRange(pet1, pet2, pet3);

    await context.SaveChangesAsync();

    context.PetOwners.AddRange(
        new PetOwner { PetOwnerId = Guid.NewGuid(), PetId = pet1.PetId, UserId = owner1.Id, IsPrimaryOwner = true },
        new PetOwner { PetOwnerId = Guid.NewGuid(), PetId = pet1.PetId, UserId = owner2.Id, IsPrimaryOwner = false },

        new PetOwner { PetOwnerId = Guid.NewGuid(), PetId = pet2.PetId, UserId = owner2.Id, IsPrimaryOwner = true },
        new PetOwner { PetOwnerId = Guid.NewGuid(), PetId = pet2.PetId, UserId = owner3.Id, IsPrimaryOwner = false },

        new PetOwner { PetOwnerId = Guid.NewGuid(), PetId = pet3.PetId, UserId = owner3.Id, IsPrimaryOwner = true },
        new PetOwner { PetOwnerId = Guid.NewGuid(), PetId = pet3.PetId, UserId = owner1.Id, IsPrimaryOwner = false }
    );

    var appointment1 = new Appointment
    {
        AppId = Guid.NewGuid(),
        AppDateTime = DateTime.UtcNow.AddDays(-10),
        AppReason = "Mr. Snuggles refused to acknowledge gravity and fell from the sofa.",
        AppStatusCodeId = 4,
        PetId = pet1.PetId,
        VetId = vet1.VetId,
        ClinicId = clinic1.ClinicId
    };

    var appointment2 = new Appointment
    {
        AppId = Guid.NewGuid(),
        AppDateTime = DateTime.UtcNow.AddDays(-4),
        AppReason = "Bark Twain ate a suspicious sock and looked proud of it.",
        AppStatusCodeId = 4,
        PetId = pet2.PetId,
        VetId = vet2.VetId,
        ClinicId = clinic1.ClinicId
    };

    var appointment3 = new Appointment
    {
        AppId = Guid.NewGuid(),
        AppDateTime = DateTime.UtcNow.AddDays(2),
        AppReason = "Lord Voldemutt has been dramatically limping only when watched.",
        AppStatusCodeId = 1,
        PetId = pet3.PetId,
        VetId = vet3.VetId,
        ClinicId = clinic2.ClinicId
    };

    context.Appointments.AddRange(appointment1, appointment2, appointment3);

    context.MedicalRecords.AddRange(
        new MedicalRecord
        {
            MedicalRecordId = Guid.NewGuid(),
            Diagnosis = "Minor sofa-related dignity injury",
            TreatmentPlan = "Rest, observation, and fewer dramatic jumps.",
            PetWeight = 5.4m,
            Comment = "Patient was offended but clinically stable.",
            AppId = appointment1.AppId,
            PetId = pet1.PetId,
            VetId = vet1.VetId
        },
        new MedicalRecord
        {
            MedicalRecordId = Guid.NewGuid(),
            Diagnosis = "Sock-related digestive concern",
            TreatmentPlan = "Monitor digestion and avoid laundry access.",
            PetWeight = 28.7m,
            Comment = "Patient showed no regret.",
            AppId = appointment2.AppId,
            PetId = pet2.PetId,
            VetId = vet2.VetId
        }
    );

    await context.SaveChangesAsync();
}

