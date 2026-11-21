using Blockchain.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Blockchain.Service
{
    public class WalletService
    {
        public Wallet CreateWallet(string name)
        {
            using var ecdsa = ECDsa.Create();
            var privateKey = ecdsa.ExportECPrivateKey();
            var publicKey = ecdsa.ExportSubjectPublicKeyInfo();
            var address = GetAddress(publicKey);
            return new Wallet(name, address, publicKey, privateKey);
        }
        public string GetAddress(byte[] publicKey)
        {
            byte[] hash = SHA256.Create().ComputeHash(publicKey);
            return Convert.ToHexString(hash).ToLower()[..10];
        }
        public bool VerifySignature(byte[] data, byte[] signature, byte[] publicKey)
        {
            using var ecdsa = ECDsa.Create();
            ecdsa.ImportSubjectPublicKeyInfo(publicKey, out _);
            return ecdsa.VerifyData(data, signature, HashAlgorithmName.SHA256);
        }
    }
}
