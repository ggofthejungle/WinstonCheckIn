using System.ComponentModel;

namespace WinstonCheckIn.Services
{
    public class CheckInService : INotifyPropertyChanged
    {
        private List<CheckInRecord> _checkInRecords = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        public IReadOnlyList<CheckInRecord> CheckInRecords => _checkInRecords.AsReadOnly();

        public void AddCheckIn(string firstName, string lastName, string phoneNumber)
        {
            // Enforce 28 person limit
            if (_checkInRecords.Count >= 28)
            {
                return; // Don't add if we've reached the limit
            }

            var record = new CheckInRecord
            {
                FirstName = firstName,
                LastName = lastName,
                Phone = phoneNumber,
                IsPaid = false // Default to not paid
            };

            _checkInRecords.Add(record);
            OnPropertyChanged(nameof(CheckInRecords));
        }

        public void UpdatePaymentStatus(int index, bool isPaid)
        {
            if (index >= 0 && index < _checkInRecords.Count)
            {
                _checkInRecords[index].IsPaid = isPaid;
                OnPropertyChanged(nameof(CheckInRecords));
            }
        }

        public void RemoveCheckIn(int index)
        {
            if (index >= 0 && index < _checkInRecords.Count)
            {
                _checkInRecords.RemoveAt(index);
                OnPropertyChanged(nameof(CheckInRecords));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CheckInRecord
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool IsPaid { get; set; }
    }
}
