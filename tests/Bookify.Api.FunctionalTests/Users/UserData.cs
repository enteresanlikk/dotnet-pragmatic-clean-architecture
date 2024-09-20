﻿using Bookify.Api.Controllers.Users;

namespace Bookify.Api.FunctionalTests.Users;

internal static class UserData
{
    public static RegisterUserRequest RegisterTestUserRequest = new(
        "test@test.com",
        "first name",
        "last name",
        "12345");
}
