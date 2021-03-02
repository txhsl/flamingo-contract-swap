﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo;
using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;

namespace FlamingoSwapPairWhiteList
{
    partial class FlamingoSwapPairWhiteList
    {
        #region Admin

#warning 检查此处的 Admin 地址是否为最新地址
        static readonly UInt160 superAdmin = "NMA2FKN8up2cEwaJgtmAiDrZWB69ApnDfp".ToScriptHash();
        const string AdminKey = nameof(superAdmin);


        // When this contract address is included in the transaction signature,
        // this method will be triggered as a VerificationTrigger to verify that the signature is correct.
        // For example, this method needs to be called when withdrawing token from the contract.
        public static bool Verify() => Runtime.CheckWitness(GetAdmin());

        /// <summary>
        /// 获取合约管理员
        /// </summary>
        /// <returns></returns>
        public static UInt160 GetAdmin()
        {
            var admin = StorageGet(AdminKey);
            return admin?.Length == 20 ? (UInt160)admin : superAdmin;
        }

        /// <summary>
        /// 设置合约管理员
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public static bool SetAdmin(UInt160 admin)
        {
            
            Assert(Runtime.CheckWitness(GetAdmin()), "Forbidden");
            StoragePut(AdminKey, admin);
            return true;
        }


        /// <summary>
        /// 升级
        /// </summary>
        /// <param name="nefFile"></param>
        /// <param name="manifest"></param>
        public static void Update(ByteString nefFile, string manifest)
        {
            if (!Verify()) throw new Exception("No authorization.");
            ContractManagement.Update(nefFile, manifest, null);
        }

        #endregion

    }
}
