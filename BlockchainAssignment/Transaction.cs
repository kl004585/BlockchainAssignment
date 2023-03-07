﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainAssignment
{
    class Transaction
    {
        DateTime timestamp; 
        public String senderAddress, recipientAddress; 
        public double amount, fee; 
        public String hash, signature; 

        public Transaction(String from, String to, double amount, double fee, String privateKey)
        {
            timestamp = DateTime.Now;

            senderAddress = from;
            recipientAddress = to;

            this.amount = amount;
            this.fee = fee;

            hash = CreateHash();
            signature = Wallet.Wallet.CreateSignature(from, privateKey, hash);
        }

      
        public String CreateHash()
        {
            String hash = String.Empty;
            SHA256 hasher = SHA256Managed.Create();

            String input = timestamp + senderAddress + recipientAddress + amount + fee;

            Byte[] hashByte = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));

            foreach (byte x in hashByte)
                hash += String.Format("{0:x2}", x);

            return hash;
        }

        public override string ToString()
        {
            return "  [TRANSACTION START]" 
                + "\n  Timestamp: " + timestamp
                + "\n  -- Verification --"
                + "\n  Hash: " + hash
                + "\n  Signature: " + signature
                + "\n  -- Quantities --"
                + "\n  Transferred: " + amount + " Nonce Coin"
                + "\t  Fee: " + fee
                + "\n  -- Participants --"
                + "\n  Sender: " + senderAddress
                + "\n  Reciever: " + recipientAddress 
                + "\n  [TRANSACTION END]";
        }
    }
}
