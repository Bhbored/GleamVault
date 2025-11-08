using PropertyChanged;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GleamVault.TestData
{
    [AddINotifyPropertyChangedInterface]
    public static class TestTransactions
    {
        private static ObservableCollection<Transaction> transactions = new();

        public static ObservableCollection<Transaction> Transactions
        {
            get => transactions;
            set
            {
                transactions = value;
            }
        }
    }
}
