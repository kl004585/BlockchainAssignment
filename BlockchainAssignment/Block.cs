using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BlockchainAssignment
{
    class Block
    {
        private DateTime timestamp; 

        private int index, 
            difficulty = 4; 

        public String prevHash,hash, merkleRoot, minerAddress; 

        public List<Transaction> transactionList; 
        
        
        public long nonce; 


        public double reward;

        public Thread mainThread;
       

        /* Genesis block constructor */
        public Block()
        {
            timestamp = DateTime.Now;
            index = 0;
            transactionList = new List<Transaction>();
            hash = Mine();


        }

        /* New Block constructor */
        public Block(Block lastBlock, List<Transaction> transactions, String minerAddress)
        {
            timestamp = DateTime.Now;

            index = lastBlock.index + 1;
            prevHash = lastBlock.hash;

            this.minerAddress = minerAddress;
            reward = 1.0; 
            transactions.Add(createRewardTransaction(transactions)); 
            transactionList = new List<Transaction>(transactions); 

            merkleRoot = MerkleRoot(transactionList); 
            hash = Mine();

            Thread thread1 = new Thread(() => CreateHash());
        }

        /* Hashes the entire Block object */
        public String CreateHash()
        {
            String hash = String.Empty;
            SHA256 hasher = SHA256Managed.Create();

           
            String input = timestamp.ToString() + index + prevHash + nonce + merkleRoot;

            
            Byte[] hashByte = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));

           
            foreach (byte x in hashByte)
                hash += String.Format("{0:x2}", x);
            
            return hash;
        }

       
        public String Mine()
        {
            nonce = 0; 
            String hash = CreateHash(); 

            String re = new string('0', difficulty);

            while(!hash.StartsWith(re))  
            {
                nonce++;
                hash = CreateHash(); 
            }

            return hash; 
        }

       
        public static String MerkleRoot(List<Transaction> transactionList)
        {
            List<String> hashes = transactionList.Select(t => t.hash).ToList(); 
            
           
            if (hashes.Count == 0) 
            {
                return String.Empty;
            }
            if (hashes.Count == 1) 
            {
                return HashCode.HashTools.combineHash(hashes[0], hashes[0]);
            }
            while (hashes.Count != 1) 
            {
                List<String> merkleLeaves = new List<String>(); 

                for (int i=0; i<hashes.Count; i+=2) 
                {
                    if (i == hashes.Count - 1)
                    {
                        merkleLeaves.Add(HashCode.HashTools.combineHash(hashes[i], hashes[i])); 
                    }
                    else
                    {
                        merkleLeaves.Add(HashCode.HashTools.combineHash(hashes[i], hashes[i + 1])); 
                    }
                }
                hashes = merkleLeaves; 
            }
            return hashes[0]; 
        }

        public Transaction createRewardTransaction(List<Transaction> transactions)
        {
            double fees = transactions.Aggregate(0.0, (acc, t) => acc + t.fee); 
            return new Transaction("Mine Rewards", minerAddress, (reward + fees), 0, ""); 
        }

      
        public override string ToString()
        {
            return "[BLOCK START]"
                + "\nIndex: " + index
                + "\tTimestamp: " + timestamp
                + "\nPrevious Hash: " + prevHash
                + "\n-- PoW --"
                + "\nDifficulty Level: " + difficulty
                + "\nNonce: " + nonce
                + "\nHash: " + hash
                + "\n-- Rewards --"
                + "\nReward: " + reward
                + "\nMiners Address: " + minerAddress
                + "\n-- " + transactionList.Count + " Transactions --"
                +"\nMerkle Root: " + merkleRoot
                + "\n" + String.Join("\n", transactionList)
                + "\n[BLOCK END]";
        }
    }
}
