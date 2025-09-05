using System.ComponentModel;

namespace WinstonCheckIn.Services
{
    public class CheckInService : INotifyPropertyChanged
    {
        private List<CheckInRecord> _checkInRecords = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        public IReadOnlyList<CheckInRecord> CheckInRecords => _checkInRecords.AsReadOnly();

        public CheckInService()
        {
            // Add organizer as the first entry
            AddOrganizer();
        }

        private void AddOrganizer()
        {
            var organizer = new CheckInRecord
            {
                FirstName = "Organizer",
                LastName = "",
                Phone = "N/A",
                IsPaid = true, // Organizer is considered paid
                WaiverAccepted = true,
                SignatureData = null,
                CheckInTime = DateTime.Now.AddDays(-1) // Set to yesterday to appear first
            };

            _checkInRecords.Add(organizer);
        }

        public void AddCheckIn(string firstName, string lastName, string phoneNumber, bool waiverAccepted = false, string? signatureData = null)
        {
            // Enforce 28 person limit (including organizer)
            if (_checkInRecords.Count >= 28)
            {
                return; // Don't add if we've reached the limit
            }

            var record = new CheckInRecord
            {
                FirstName = firstName,
                LastName = lastName,
                Phone = phoneNumber,
                IsPaid = false, // Default to not paid
                WaiverAccepted = waiverAccepted,
                SignatureData = signatureData,
                CheckInTime = DateTime.Now
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
                // Prevent deleting the organizer (first entry)
                if (index == 0 && _checkInRecords[0].FirstName == "Organizer")
                {
                    return; // Don't delete the organizer
                }
                
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
        public bool WaiverAccepted { get; set; }
        public string? SignatureData { get; set; } // Base64 encoded signature image
        public DateTime CheckInTime { get; set; } = DateTime.Now;
    }
}
