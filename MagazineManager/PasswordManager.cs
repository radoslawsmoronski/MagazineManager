﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using BCrypt.Net;

namespace MagazineManager
{
    internal class PasswordManager
    {
        public static string HashPassword(SecureString password)
        {
            IntPtr valuePtr = IntPtr.Zero;

            try
            {
                // Convert SecureString to plain text
                valuePtr = Marshal.SecureStringToBSTR(password);
                string plainTextPassword = Marshal.PtrToStringBSTR(valuePtr);

                // Hash the plain text password using BCrypt
                return BCrypt.Net.BCrypt.HashPassword(plainTextPassword, BCrypt.Net.BCrypt.GenerateSalt());
            }
            finally
            {
                // Securely clean up memory used for plain text password
                if (valuePtr != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(valuePtr);
                }
            }
        }

        public static bool VerifyPassword(SecureString enteredPassword, string hashedPassword)
        {
            IntPtr valuePtr = IntPtr.Zero;

            try
            {
                // Convert SecureString to plain text
                valuePtr = Marshal.SecureStringToBSTR(enteredPassword);
                string plainTextEnteredPassword = Marshal.PtrToStringBSTR(valuePtr);

                // Verify the plain text entered password against the hashed password using BCrypt
                return BCrypt.Net.BCrypt.Verify(plainTextEnteredPassword, hashedPassword);
            }
            finally
            {
                // Securely clean up memory used for plain text entered password
                if (valuePtr != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(valuePtr);
                }
            }

        }

        public static SecureString GetSecurePassword(PasswordBox passwordBox)
        {
            SecureString securePassword = new SecureString();

            if (passwordBox.SecurePassword != null)
            {
                IntPtr unmanagedPointer = IntPtr.Zero;

                try
                {
                    unmanagedPointer = Marshal.SecureStringToGlobalAllocUnicode(passwordBox.SecurePassword);

                    for (int i = 0; i < passwordBox.SecurePassword.Length; i++)
                    {
                        char c = (char)Marshal.PtrToStructure(unmanagedPointer + i * 2, typeof(char));
                        securePassword.AppendChar(c);
                    }
                }
                finally
                {
                    if (unmanagedPointer != IntPtr.Zero)
                    {
                        Marshal.ZeroFreeGlobalAllocUnicode(unmanagedPointer);
                    }
                }
            }

            return securePassword;
        }
    }
}