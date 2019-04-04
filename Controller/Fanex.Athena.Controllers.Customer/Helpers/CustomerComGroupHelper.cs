using System;
using System.Collections.Generic;
using System.Linq;
using Fanex.Athena.Model.Customer.MetaModel;
using Fanex.Athena.Resources;
using Fanex.Athena.Service.Customer.Helpers;

namespace Fanex.Athena.Controllers.Customer.Helpers
{
    public static class CustomerComGroupHelper
    {
        private static readonly IDictionary<int, string> UpdatingErrorMap = new Dictionary<int, string>
        {
            { 100, nameof(ViewResources.MessageCommCannotGreaterThan) },
            { 101, nameof(ViewResources.MessageGroupDoesNotExistMsg) },
            { 102, nameof(ViewResources.MessageGroupHasAlreadyChoosen) },
            { 103, nameof(ViewResources.GroupSetAsDefaultMsg) },
        };

        private static readonly IDictionary<int, string> CreatingErrorMap = new Dictionary<int, string>
        {
            { 100, nameof(ViewResources.MessageCommCannotGreaterThan) },
            { 101, nameof(ViewResources.MessageGroupCreationFail) },
            { 102, nameof(ViewResources.GroupNameExistedMsg) }
        };

        public static bool IsValidCommissions(GroupCommMetaModel metaModel)
        {
            return GroupCommHelper.IsAllValid(
                                metaModel.SBComm,
                                metaModel.ParlayComm,
                                metaModel.NGComm,
                                metaModel.CSComm);
        }

        public static string GetUpdateGroupErrorMessage(int errorCode)
        {
            if (!UpdatingErrorMap.ContainsKey(errorCode))
            {
                return string.Empty;
            }

            return ViewResources.ResourceManager.GetString(UpdatingErrorMap[errorCode]);
        }

        public static string GetCreateGroupErrorMessage(int errorCode)
        {
            if (!CreatingErrorMap.ContainsKey(errorCode))
            {
                return string.Empty;
            }

            return ViewResources.ResourceManager.GetString(CreatingErrorMap[errorCode]);
        }

#pragma warning disable S109 // Magic numbers should not be used
        public static int SetComm(decimal comm, int position)
        {
            var actualComm = comm * 10000;
            switch (position)
            {
                case 1:
                    return (int)(actualComm / 100);

                case 2:
                    return (int)(actualComm % 100) / 10;

                case 3:
                    return (int)actualComm % 10;

                default:
                    return 0;
            }
        }

        /// <summary>
        ///  Repair commission values in order to binding them to drop down list control.
        /// </summary>
        /// <param name="model">The group commission metal model.</param>
        public static void SetDropDownListComm(GroupCommMetaModel model)
        {
            var decSbComm = model.SBComm;
            var decParlayComm = model.ParlayComm;
            var decNgComm = model.NGComm;
            var decCsComm = model.CSComm;

            model.SBComm1 = SetComm(decSbComm, 1);
            model.SBComm2 = SetComm(decSbComm, 2);
            model.SBComm3 = SetComm(decSbComm, 3);

            model.ParlayComm1 = SetComm(decParlayComm, 1);
            model.ParlayComm2 = SetComm(decParlayComm, 2);
            model.ParlayComm3 = SetComm(decParlayComm, 3);

            model.NGComm1 = SetComm(decNgComm, 1);
            model.NGComm2 = SetComm(decNgComm, 2);
            model.NGComm3 = SetComm(decNgComm, 3);

            model.CSComm1 = SetComm(decCsComm, 1);
            model.CSComm2 = SetComm(decCsComm, 2);
            model.CSComm3 = SetComm(decCsComm, 3);
        }
#pragma warning restore S109 // Magic numbers should not be used

        public static void UpdateMetaModelAfterProcessSuccessfully(GroupCommMetaModel metaModel)
        {
            metaModel.IsResetDefault = false;
            metaModel.DefaultGroupId = metaModel.IsDefaultGroup ? metaModel.GroupCommId : metaModel.DefaultGroupId;
            metaModel.OriginalGroupName = metaModel.GroupName;
        }

        public static bool IsSameGroup(GroupCommMetaModel currentGroup, GroupCommMetaModel editedGroup)
        {
            var currentComm = BuildCommissionList(currentGroup);

            var editedComm = BuildCommissionList(editedGroup);

            for (var i = 0; i < currentComm.Count(); i++)
            {
                if (currentComm[i] != editedComm[i])
                {
                    return false;
                }
            }

            return currentGroup.GroupColor.Equals(editedGroup.GroupColor, StringComparison.OrdinalIgnoreCase)
                && currentGroup.GroupCommId == editedGroup.GroupCommId
                && currentGroup.GroupName.Equals(editedGroup.GroupName)
                && currentGroup.IsDefaultGroup == editedGroup.IsDefaultGroup;
        }

        private static decimal[] BuildCommissionList(GroupCommMetaModel groupComm)
        {
            return new[]
           {
                groupComm.SBComm,
                groupComm.ParlayComm,
                groupComm.NGComm,
                groupComm.CSComm,
            };
        }
    }
}