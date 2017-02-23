using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MPAi.Modules
{
    /// <summary>
    /// Maintains details of all of the users of the system, the current user, and the administrator.
    /// Only one file and list of users should exist at a time, so this class is static.
    /// </summary>
    public static class UserManagement
    {
        private static MPAiUser currentUser = null; // Make sure that current user has the correct voice type when you assign to it.
        private static string userDirRoot = AppDataPath.Path;
        private static string fileName = "AppSettings.dat";
        private static readonly string adminName = "admin";

        private static List<MPAiUser> allUsers;

        public static MPAiUser CurrentUser
        {
            get
            {
                return currentUser;
            }
        }

        /// <summary>
        /// Constructor for the UserManagement class, restores current user's settings, all users, and creates an admin user if one doesn't already exist.
        /// </summary>
        static UserManagement()
        {
            allUsers = new List<MPAiUser>();
            ReadSettings();

            if (allUsers.Count == 0)
            {
                allUsers.Add(new MPAiUser(adminName, adminName, new VoiceType(GenderType.MASCULINE, LanguageType.NATIVE), true, true));
            }
        }

        /// <summary>
        /// Returns the users of the system.
        /// </summary>
        /// <returns>A list of all the MPAiUsers.</returns>
        public static List<MPAiUser> GetAllUsers()
        {
            return allUsers;
        }

        /// <summary>
        /// Gets a single user by their unique username.
        /// </summary>
        /// <param name="username">The name of the user to get.</param>
        /// <returns>The MPAiUser with name username.</returns>
        public static MPAiUser GetUser(string username)
        {
            return GetAllUsers().Find(x => x.getName().Equals(username.ToLower()));
        }

        /// <summary>
        /// Checks if a user already exists with the same username as the one input, and adds the user to the database if not.
        /// </summary>
        /// <param name="newUserName">The username of the new user.</param>
        /// <param name="newCode">The password of the new user.</param>
        /// <returns>True if the creation was successful, false if not.</returns>
        public static bool CreateNewUser(string newUserName, string newCode)
        {
            IEnumerable<MPAiUser> searchUser =
                from user in allUsers
                where user.getName() == newUserName.ToLower()
                select user;

            if (searchUser.Count() == 0)
            {
                allUsers.Add(new MPAiUser(newUserName, newCode));
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks whether the input user already exists in the system.
        /// </summary>
        /// <param name="candidate">The user to check, as an MPAiUser.</param>
        /// <returns>True if the user is in the system, false if not.</returns>
        public static bool ContainsUser(MPAiUser candidate)
        {
            return ContainsUser(candidate.getName());
        }

        /// <summary>
        /// Checks whether the input user already exists in the system.
        /// </summary>
        /// <param name="candidate">The username to check, as a string.</param>
        /// <returns>True if the user is in the system, false if not.</returns>
        public static bool ContainsUser(string Username)
        {
            foreach (MPAiUser item in allUsers)
            {
                if (item.getName() == Username)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if a user already exists with the same username as the user input, and adds the user to the database if not.
        /// </summary>
        /// <param name="candidate">The user to add to the system, as an MPAiUser.</param>
        /// <returns>True if the creation was successful, false if not.</returns>
        public static bool CreateNewUser(MPAiUser candidate)
        {
            if (allUsers.Contains(candidate))
                return false;
            else
            {
                allUsers.Add(candidate);
                return true;
            }
        }

        /// <summary>
        /// Checks if the current user is the administrator.
        /// </summary>
        /// <returns>True if the current user is the administrator, false if not.</returns>
        public static bool IsCurrentUserAdmin()
        {
            return CurrentUser.IsAdmin;
        }

        /// <summary>
        /// Checks if the input user is a valid, existing user, with a correct password, and sets them to the current user. 
        /// Also adds the stored voicetype to the user object that is authenticated.
        /// </summary>
        /// <param name="tUser">The user to authenticate, as an MPAiUser object. This is passed by reference.</param>
        /// <returns>True if the user exists already, false otherwise.</returns>
        public static bool AuthenticateUser(ref MPAiUser tUser)
        {
            // This changes the field, as the property's setter is designed to be used from outside the class, and would cause this to break.
            if (allUsers.Contains(tUser) && GetUser(tUser.getName()).codeCorrect(tUser.getCode()))
            {
                MPAiUser user = GetUser(tUser.getName());
                currentUser = user;    // Set the user as the current user.
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Changes the current user's password.
        /// </summary>
        /// <param name="userName">The username of the user to change, as a string.</param>
        /// <param name="newCode">The new password for the specified user, as a string.</param>
        public static void ChangeCurrentUserCode(string newCode)
        {
            CurrentUser.UserPswd = newCode;
        }

        /// <summary>
        /// Removes the specified user from the system.
        /// </summary>
        /// <param name="userToRemove">The user to remove, as an MPAiUser object.</param>
        public static void RemoveUser(MPAiUser userToRemove)
        {
            allUsers.Remove(userToRemove);
        }

        /// <summary>
        /// Returns a string representing the settings file on the user's computer.
        /// </summary>
        /// <returns>The path to the settings file, as a string.</returns>
        private static string GetSettingFilePath()
        {
            return (Path.Combine(userDirRoot, fileName));
        }

        /// <summary>
        /// Opens the settings file and populates the allUsers field with the data found there.
        /// Also restores the last user to access the system to current user status.
        /// </summary>
        public static void ReadSettings()
        {
            try
            {
                using (FileStream fs = new FileStream(GetSettingFilePath(),FileMode.OpenOrCreate, FileAccess.Read))
                {
                    using (BinaryReader reader = new BinaryReader(fs))
                    {
                        if (new FileInfo(GetSettingFilePath()).Length != 0) // If the file wasn't just created
                        {
                            int n = reader.ReadInt32();
                            for (int i = 0; i < n; i++)
                            {
                                allUsers.Add(new MPAiUser(reader.ReadString(), reader.ReadString(), VoiceType.getVoiceTypeFromString(reader.ReadString()), reader.ReadBoolean(), reader.ReadBoolean()));
                            }
                            // restore the last used user, if there was one.

                            int index = reader.ReadInt32();
                            if (index >= 0 && index < allUsers.Count)
                            {
                                currentUser = allUsers[index];
                            }
                        }                       
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        /// <summary>
        /// Writes all users to the settings file, and creates it if it does not already exist.
        /// </summary>
        public static void WriteSettings()
        {
            Int32 n = allUsers.Count;

            try
            {
                using (FileStream fs = new FileStream(GetSettingFilePath(), FileMode.Create))
                {
                    using (BinaryWriter writer = new BinaryWriter(fs))
                    {
                        writer.Write(n);
                        if (!(n == 0))
                        {
                            foreach (MPAiUser user in allUsers)
                            {
                                writer.Write(user.getName());
                                writer.Write(user.getCode());
                                writer.Write(VoiceType.getStringFromVoiceType(user.Voice));
                                writer.Write(user.IsAdmin);
                                writer.Write(user.OriginalAdmin);
                            }         
                            if (CurrentUser == null)    // If there is a current user, store it.
                            {
                                writer.Write((Int32)(-1));
                            }
                            else
                            { 
                                writer.Write((Int32)(allUsers.FindIndex(CurrentUser.Equals)));
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }
    }
}
