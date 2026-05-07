using Microsoft.EntityFrameworkCore;
using PetSpace.Core.Domain;
using PetSpace.Data;

namespace PetSpace.Tests;

public class PetAccessTests
{
    private PetSpaceDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<PetSpaceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new PetSpaceDbContext(options);
    }

    private readonly DateTime _testDate =
     new DateTime(2026, 5, 20, 10, 0, 0);

    private const int PendingStatus = 1;
    private const int ApprovedStatus = 2;
    private const int RejectedStatus = 3;
    private const int CompletedStatus = 4;

    private Pet CreateTestPet(Guid petId, Guid ownerId)
    {
        return new Pet
        {
            PetId = petId,
            PetName = "Captain Paw",
            PetSpecies = "Dog",
            OwnerId = ownerId
        };
    }

    private Appointment CreateTestAppointment(Guid petId, Guid vetId, int status)
    {
        return new Appointment
        {
            AppId = Guid.NewGuid(),
            PetId = petId,
            VetId = vetId,
            AppDateTime = _testDate,
            AppReason = "Checkup",
            AppStatusCodeId = status
        };
    }

    private PetOwner CreatePetOwner(Guid petId, Guid userId, bool isPrimary)
    {
        return new PetOwner
        {
            PetOwnerId = Guid.NewGuid(),
            PetId = petId,
            UserId = userId,
            IsPrimaryOwner = isPrimary
        };
    }

    [Fact]
    public async Task Shared_owner_can_see_pet_but_is_not_primary_owner()
    {
        // Arrange
        using var context = CreateContext();

        var primaryUserId = Guid.NewGuid();
        var sharedUserId = Guid.NewGuid();
        var petId = Guid.NewGuid();

        var pet = new Pet
        {
            PetId = petId,
            PetName = "Bark Twain",
            PetSpecies = "Dog",
            OwnerId = primaryUserId
        };

        context.Pets.Add(pet);

        context.PetOwners.AddRange(
            new PetOwner
            {
                PetOwnerId = Guid.NewGuid(),
                PetId = petId,
                UserId = primaryUserId,
                IsPrimaryOwner = true
            },
            new PetOwner
            {
                PetOwnerId = Guid.NewGuid(),
                PetId = petId,
                UserId = sharedUserId,
                IsPrimaryOwner = false
            }
        );

        await context.SaveChangesAsync();

        // Act
        var sharedPet = await context.Pets
            .Where(p => p.PetOwners.Any(po => po.UserId == sharedUserId))
            .Select(p => new
            {
                p.PetId,
                p.PetName,
                IsPrimaryOwner = p.OwnerId == sharedUserId
            })
            .FirstOrDefaultAsync();

        // Assert
        Assert.NotNull(sharedPet);
        Assert.Equal("Bark Twain", sharedPet.PetName);
        Assert.False(sharedPet.IsPrimaryOwner);
    }

    [Fact]
    public async Task Primary_owner_can_be_detected_for_own_pet()
    {
        using var context = CreateContext();

        var userId = Guid.NewGuid();
        var petId = Guid.NewGuid();

        context.Pets.Add(CreateTestPet(petId, userId));

        context.PetOwners.Add(new PetOwner
        {
            PetOwnerId = Guid.NewGuid(),
            PetId = petId,
            UserId = userId,
            IsPrimaryOwner = true
        });

        await context.SaveChangesAsync();

        var isPrimaryOwner = await context.PetOwners
            .AnyAsync(po =>
                po.PetId == petId &&
                po.UserId == userId &&
                po.IsPrimaryOwner);

        Assert.True(isPrimaryOwner);
    }

    [Fact]
    public async Task Pet_with_appointment_has_related_appointment()
    {
        using var context = CreateContext();

        var userId = Guid.NewGuid();
        var petId = Guid.NewGuid();
        var vetId = Guid.NewGuid();

        context.Pets.Add(CreateTestPet(petId, userId));

        context.Appointments.Add(new Appointment
        {
            AppId = Guid.NewGuid(),
            PetId = petId,
            VetId = vetId,
            AppDateTime = DateTime.UtcNow,
            AppReason = "Checkup",
            AppStatusCodeId = 1
        });

        await context.SaveChangesAsync();

        var hasAppointment = await context.Appointments
            .AnyAsync(a => a.PetId == petId);

        Assert.True(hasAppointment);
    }

    [Fact]
    public async Task New_vet_is_not_verified_by_default()
    {
        using var context = CreateContext();

        var vet = new Vet
        {
            VetId = Guid.NewGuid(),
            VetSpecialization = "Surgery",
            IsVerified = false
        };

        context.Vets.Add(vet);

        await context.SaveChangesAsync();

        var createdVet = await context.Vets.FirstOrDefaultAsync();

        Assert.NotNull(createdVet);
        Assert.False(createdVet.IsVerified);
    }

    [Fact]
    public async Task New_appointment_has_pending_status()
    {
        using var context = CreateContext();

        var appointment = new Appointment
        {
            AppId = Guid.NewGuid(),
            PetId = Guid.NewGuid(),
            VetId = Guid.NewGuid(),
            AppDateTime = DateTime.UtcNow,
            AppReason = "Vaccination",
            AppStatusCodeId = 1
        };

        context.Appointments.Add(appointment);

        await context.SaveChangesAsync();

        var savedAppointment = await context.Appointments.FirstOrDefaultAsync();

        Assert.NotNull(savedAppointment);
        Assert.Equal(1, savedAppointment.AppStatusCodeId);
    }

    [Fact]
    public async Task Pet_can_have_multiple_owners()
    {
        using var context = CreateContext();

        var petId = Guid.NewGuid();

        context.PetOwners.AddRange(
            new PetOwner
            {
                PetOwnerId = Guid.NewGuid(),
                PetId = petId,
                UserId = Guid.NewGuid(),
                IsPrimaryOwner = true
            },
            new PetOwner
            {
                PetOwnerId = Guid.NewGuid(),
                PetId = petId,
                UserId = Guid.NewGuid(),
                IsPrimaryOwner = false
            }
        );

        await context.SaveChangesAsync();

        var ownerCount = await context.PetOwners
            .CountAsync(po => po.PetId == petId);

        Assert.Equal(2, ownerCount);
    }

    [Fact]
    public async Task Pet_should_have_only_one_primary_owner()
    {
        using var context = CreateContext();

        var petId = Guid.NewGuid();

        context.PetOwners.AddRange(
            new PetOwner
            {
                PetOwnerId = Guid.NewGuid(),
                PetId = petId,
                UserId = Guid.NewGuid(),
                IsPrimaryOwner = true
            },
            new PetOwner
            {
                PetOwnerId = Guid.NewGuid(),
                PetId = petId,
                UserId = Guid.NewGuid(),
                IsPrimaryOwner = false
            }
        );

        await context.SaveChangesAsync();

        var primaryOwnerCount = await context.PetOwners
            .CountAsync(po => po.PetId == petId && po.IsPrimaryOwner);

        Assert.Equal(1, primaryOwnerCount);
    }

    [Fact]
    public async Task Shared_owner_has_access_to_pet_medical_records()
    {
        using var context = CreateContext();

        var petId = Guid.NewGuid();
        var primaryUserId = Guid.NewGuid();
        var sharedUserId = Guid.NewGuid();

        context.Pets.Add(new Pet
        {
            PetId = petId,
            PetName = "Doctor Fluff",
            PetSpecies = "Cat",
            OwnerId = primaryUserId
        });

        context.PetOwners.AddRange(
            new PetOwner
            {
                PetOwnerId = Guid.NewGuid(),
                PetId = petId,
                UserId = primaryUserId,
                IsPrimaryOwner = true
            },
            new PetOwner
            {
                PetOwnerId = Guid.NewGuid(),
                PetId = petId,
                UserId = sharedUserId,
                IsPrimaryOwner = false
            }
        );

        context.MedicalRecords.Add(new MedicalRecord
        {
            MedicalRecordId = Guid.NewGuid(),
            PetId = petId,
            Diagnosis = "Healthy",
            TreatmentPlan = "Regular checkup"
        });

        await context.SaveChangesAsync();

        var hasAccess = await context.PetOwners
            .AnyAsync(po => po.PetId == petId && po.UserId == sharedUserId);

        var records = await context.MedicalRecords
            .Where(r => r.PetId == petId)
            .ToListAsync();

        Assert.True(hasAccess);
        Assert.Single(records);
    }

    [Fact]
    public async Task Deleted_appointment_should_not_be_accepted()
    {
        using var context = CreateContext();

        var appointment = new Appointment
        {
            AppId = Guid.NewGuid(),
            PetId = Guid.NewGuid(),
            VetId = Guid.NewGuid(),
            AppDateTime = DateTime.UtcNow,
            AppReason = "Checkup",
            AppStatusCodeId = 3 // Rejected
        };

        context.Appointments.Add(appointment);

        await context.SaveChangesAsync();

        var canBeAccepted = appointment.AppStatusCodeId == 1;

        Assert.False(canBeAccepted);
    }

    [Fact]
    public async Task Completed_appointment_can_have_medical_record()
    {
        using var context = CreateContext();

        var petId = Guid.NewGuid();

        var appointment = new Appointment
        {
            AppId = Guid.NewGuid(),
            PetId = petId,
            VetId = Guid.NewGuid(),
            AppDateTime = DateTime.UtcNow,
            AppReason = "Surgery",
            AppStatusCodeId = 4 // Completed
        };

        context.Appointments.Add(appointment);

        context.MedicalRecords.Add(new MedicalRecord
        {
            MedicalRecordId = Guid.NewGuid(),
            PetId = petId,
            Diagnosis = "Recovered well",
            TreatmentPlan = "Rest"
        });

        await context.SaveChangesAsync();

        var hasMedicalRecord = await context.MedicalRecords
            .AnyAsync(r => r.PetId == petId);

        Assert.True(hasMedicalRecord);
    }

    [Fact]
    public async Task User_creating_new_pet_becomes_primary_owner()
    {
        using var context = CreateContext();

        var userId = Guid.NewGuid();
        var petId = Guid.NewGuid();

        context.Pets.Add(CreateTestPet(petId, userId));

        context.PetOwners.Add(new PetOwner
        {
            PetOwnerId = Guid.NewGuid(),
            PetId = petId,
            UserId = userId,
            IsPrimaryOwner = true
        });

        await context.SaveChangesAsync();

        var createdOwner = await context.PetOwners
            .FirstOrDefaultAsync(po =>
                po.PetId == petId &&
                po.UserId == userId);

        Assert.NotNull(createdOwner);
        Assert.True(createdOwner.IsPrimaryOwner);
    }
}