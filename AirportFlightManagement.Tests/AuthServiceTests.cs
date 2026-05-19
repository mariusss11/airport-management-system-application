using BCrypt.Net;
using Xunit;

namespace AirportFlightManagement.Tests;

// Tests for AuthService static helpers that have no DB dependency.
public class AuthServiceTests
{
    [Fact]
    public void HashPassword_ProducesVerifiableHash()
    {
        const string password = "password";
        var hash = BCrypt.Net.BCrypt.HashPassword(password);

        Assert.NotNull(hash);
        Assert.NotEqual(password, hash);
        Assert.True(BCrypt.Net.BCrypt.Verify(password, hash));
    }

    [Fact]
    public void HashPassword_WrongPassword_FailsVerification()
    {
        var hash = BCrypt.Net.BCrypt.HashPassword("correct");
        Assert.False(BCrypt.Net.BCrypt.Verify("wrong", hash));
    }

    [Fact]
    public void HashPassword_TwoCallsSamePlaintext_ProduceDifferentHashes()
    {
        // BCrypt uses a random salt each time
        var hash1 = BCrypt.Net.BCrypt.HashPassword("secret");
        var hash2 = BCrypt.Net.BCrypt.HashPassword("secret");

        Assert.NotEqual(hash1, hash2);
        Assert.True(BCrypt.Net.BCrypt.Verify("secret", hash1));
        Assert.True(BCrypt.Net.BCrypt.Verify("secret", hash2));
    }

    [Fact]
    public void HashPassword_EmptyString_DoesNotThrow()
    {
        var hash = BCrypt.Net.BCrypt.HashPassword("");
        Assert.True(BCrypt.Net.BCrypt.Verify("", hash));
    }
}
