// -----------------------------------------------------------------------
// <copyright file="User.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.Models.DTO.Common;

    /// <summary>
    /// User class.
    /// </summary>
    [Serializable]
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Role { get; set; }

        public string Status { get; set; }

        public int ServerId { get; set; }

        public string Functions { get; set; }

        public bool IsInternal { get; set; }

        public Licensee Licensee { get; set; }

        public bool IsManageAccount { get; set; }

        public IRequest Request { get; set; }

        public List<string> FuntionList
        {
            get
            {
                var functionList = new List<string>();
                if (!string.IsNullOrEmpty(Functions))
                {
                    string functions = Functions.Trim();
                    if (functions.Contains(','))
                    {
                        functionList = functions.Split(',').ToList();
                    }
                    else
                    {
                        functionList.Add(functions);
                    }
                }

                return functionList;
            }
        }

        public bool HasPermission(string function)
        {
            return !string.IsNullOrEmpty(function) && FuntionList.Contains(function);
        }

        public bool IsInRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return false;
            }

            return Role.Equals(roleName, StringComparison.OrdinalIgnoreCase);
        }
    }
}