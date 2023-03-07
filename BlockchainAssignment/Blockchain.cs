using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainAssignment
{
    class Blockchain
    {
        public List<Block> blocks;

        private int transactionsPerBlock = 5;

        public List<Transaction> transactionPool = new List<Transaction>();

        public Blockchain()
        {
            blocks = new List<Block>()
            {
                new Block()
            };
        }

        public String GetBlockAsString(int index)
        {
            if (index >= 0 && index < blocks.Count)
                return blocks[index].ToString();
            else
                return "No such block exists";
        }

        public Block GetLastBlock()
        {
            return blocks[blocks.Count - 1];
        }

        public List<Transaction> GetPendingTransactions()
        {
            int n = Math.Min(transactionsPerBlock, transactionPool.Count);

            List<Transaction> transactions = transactionPool.GetRange(0, n);
            transactionPool.RemoveRange(0, n);

            return transactions;
        }

        public static bool ValidateHash(Block b)
        {
            String rehash = b.CreateHash();
            return rehash.Equals(b.hash);
        }

        public static bool ValidateMerkleRoot(Block b)
        {
            String reMerkle = Block.MerkleRoot(b.transactionList);
            return reMerkle.Equals(b.merkleRoot);
        }

        public double GetBalance(String address)
        {
            double balance = 0;

            foreach(Block b in blocks)
            {
                foreach(Transaction t in b.transactionList)
                {
                    if (t.recipientAddress.Equals(address))
                    {
                        balance += t.amount;
                    }
                    if (t.senderAddress.Equals(address))
                    {
                        balance -= (t.amount + t.fee); 
                    }
                }
            }
            return balance;
        }

        public override string ToString()
        {
            return String.Join("\n", blocks);
        }
    }
}
