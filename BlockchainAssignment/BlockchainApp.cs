using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace BlockchainAssignment
{
    public partial class BlockchainApp : Form
    {
        private Blockchain blockchain;

        private Stopwatch stopwatch;

        public BlockchainApp()
        {
            InitializeComponent();
            blockchain = new Blockchain();
            stopwatch = new Stopwatch();
            UpdateText("New blockchain initialised!");
        }

        private void UpdateText(String text)
        {
            output.Text = text;
        }

        private void UpdateTimer(String text)
        {
            timer.Text = text;
        }

        private void ReadAll_Click(object sender, EventArgs e)
        {
            UpdateText(blockchain.ToString());
        }

        private void PrintBlock_Click(object sender, EventArgs e)
        {
            if (Int32.TryParse(blockNo.Text, out int index))
                UpdateText(blockchain.GetBlockAsString(index));
            else
                UpdateText("Invalid Block No.");
        }

        private void PrintPendingTransactions_Click(object sender, EventArgs e)
        {
            UpdateText(String.Join("\n", blockchain.transactionPool));
        }

        private void GenerateWallet_Click(object sender, EventArgs e)
        {
            Wallet.Wallet myNewWallet = new Wallet.Wallet(out string privKey);

            publicKey.Text = myNewWallet.publicID;
            privateKey.Text = privKey;
        }

        private void ValidateKeys_Click(object sender, EventArgs e)
        {
            if (Wallet.Wallet.ValidatePrivateKey(privateKey.Text, publicKey.Text))
                UpdateText("Keys are valid");
            else
                UpdateText("Keys are invalid");
        }

        private void CheckBalance_Click(object sender, EventArgs e)
        {
            UpdateText(blockchain.GetBalance(publicKey.Text).ToString() + " Nonce Coin");
        }


        private void CreateTransaction_Click(object sender, EventArgs e)
        {
            Transaction transaction = new Transaction(publicKey.Text, reciever.Text, Double.Parse(amount.Text), Double.Parse(fee.Text), privateKey.Text);
            blockchain.transactionPool.Add(transaction);
            UpdateText(transaction.ToString());
        }

        private void NewBlock_Click(object sender, EventArgs e)
        {
            stopwatch.Start();
            List<Transaction> transactions = blockchain.GetPendingTransactions();

            Block newBlock = new Block(blockchain.GetLastBlock(), transactions, publicKey.Text);
            blockchain.blocks.Add(newBlock);

            stopwatch.Stop();
            TimeSpan timeTaken = stopwatch.Elapsed;
            string totalTime = timeTaken.ToString(@"m\:ss\.fff");
            UpdateTimer(totalTime);
            stopwatch.Reset();

            UpdateText(blockchain.ToString());
        }


        private void Validate_Click(object sender, EventArgs e)
        {
            if(blockchain.blocks.Count == 1)
            {
                if (!Blockchain.ValidateHash(blockchain.blocks[0])) 
                    UpdateText("Blockchain is invalid");
                else
                    UpdateText("Blockchain is valid");
                return;
            }

            for (int i=1; i<blockchain.blocks.Count-1; i++)
            {
                if(
                    blockchain.blocks[i].prevHash != blockchain.blocks[i - 1].hash || 
                    !Blockchain.ValidateHash(blockchain.blocks[i]) ||  
                    !Blockchain.ValidateMerkleRoot(blockchain.blocks[i]) 
                )
                {
                    UpdateText("Blockchain is invalid");
                    return;
                }
            }
            UpdateText("Blockchain is valid");
        }

        private void greedy_Click(object sender, EventArgs e)
        {

        }

        private void altruistic_Click(object sender, EventArgs e)
        {

        }

        private void random_Click(object sender, EventArgs e)
        {

        }
    }
}