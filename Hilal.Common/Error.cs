using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.Common
{
    public static class Error
    {
        public static string AccessDenied { get; } = "Access Denied!";
        public static string InvalidToken { get; } = "Invalid Token!";
        public static string MissingAuthorization { get; } = "Missing 'Authorization' header. Access denied!";
        public static string TokenExpired { get; } = "Access Denied. Token Expired!";
        public static string AccoutNotVerified { get; } = "Please verify your account first.";
        public static string ServerError { get; } = "Something went wrong.";
        public static string PhoneAlreadyExists { get; } = "Phone Number already exists!";
        public static string EmailAlreadyExists { get; } = "Email already exists!";
        public static string UserNotSubscribed { get; } = "You are not subscribed!";
        public static string UserPlanExpired { get; } = "Your plan is expired!";
        public static string UserOfferLimitExceeded { get; } = "Your offer limit exceeded!";
        public static string InvalidPincode { get; } = "Invalid Pincode!";
        public static string PhoneDoesnotExists { get; } = "Your entered phone number doesn't exist";
        public static string WrongPassword { get; } = "Wrong password!";
        public static string UserDoesnotExists { get; } = "User doesn’t exist.";
        public static string FileNotFound { get; } = "File not found!";
        public static string LoginFailed { get; } = "Login Failed!";
        public static string AccountBlocked { get; } = "Your account has been blocked!";
        public static string InvalidId { get; } = "Invalid Id!";
    }
}
