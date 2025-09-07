using System.ComponentModel;
using Microsoft.JSInterop;
using System.Text.Json;

namespace WinstonCheckIn.Services
{
    public class CheckInService : INotifyPropertyChanged
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly List<CheckInRecord> _checkInRecords = new();
        private int? _currentEventId;

        public event PropertyChangedEventHandler? PropertyChanged;

        public IReadOnlyList<CheckInRecord> CheckInRecords => _checkInRecords.AsReadOnly();
        public int? CurrentEventId => _currentEventId;

        public CheckInService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SetCurrentEventAsync(int eventId)
        {
            _currentEventId = eventId;
            await LoadCheckInRecordsAsync();
        }

        public async Task LoadCheckInRecordsAsync()
        {
            if (_currentEventId.HasValue)
            {
                try
                {
                    var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", $"checkins_event_{_currentEventId}");
                    if (!string.IsNullOrEmpty(json))
                    {
                        _checkInRecords.Clear();
                        var records = JsonSerializer.Deserialize<List<CheckInRecord>>(json) ?? new List<CheckInRecord>();
                        _checkInRecords.AddRange(records);
                    }
                    else
                    {
                        _checkInRecords.Clear();
                        // No organizer placeholder - admin will be first to check in
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading check-in records: {ex.Message}");
                    _checkInRecords.Clear();
                    // No organizer placeholder - admin will be first to check in
                }
            }
            OnPropertyChanged(nameof(CheckInRecords));
        }


        public async Task AddCheckIn(string firstName, string lastName, string phone, bool waiverAccepted, string? signatureData)
        {
            if (!_currentEventId.HasValue)
            {
                Console.WriteLine("No current event set");
                return;
            }

            // Check if we've reached the limit (28 people including organizer)
            if (_checkInRecords.Count >= 28)
            {
                return; // Don't add if limit reached
            }

            var record = new CheckInRecord
            {
                FirstName = firstName,
                LastName = lastName,
                Phone = phone,
                IsPaid = false, // Default to not paid
                WaiverAccepted = waiverAccepted,
                SignatureData = signatureData,
                CheckInTime = DateTime.Now
            };

            _checkInRecords.Add(record);
            await SaveCheckInRecordsAsync();
            OnPropertyChanged(nameof(CheckInRecords));
        }

        public async Task RemoveCheckIn(int index)
        {
            if (index >= 0 && index < _checkInRecords.Count)
            {
                _checkInRecords.RemoveAt(index);
                await SaveCheckInRecordsAsync();
                OnPropertyChanged(nameof(CheckInRecords));
            }
        }

        public async Task TogglePaymentStatus(int index)
        {
            if (index >= 0 && index < _checkInRecords.Count)
            {
                _checkInRecords[index].IsPaid = !_checkInRecords[index].IsPaid;
                await SaveCheckInRecordsAsync();
                OnPropertyChanged(nameof(CheckInRecords));
            }
        }


        public async Task ClearAllCheckIns()
        {
            _checkInRecords.Clear();
            await SaveCheckInRecordsAsync();
            OnPropertyChanged(nameof(CheckInRecords));
        }

        public async Task ArchiveEventDataAsync(int eventId)
        {
            try
            {
                var archiveData = new
                {
                    EventId = eventId,
                    ArchivedAt = DateTime.Now,
                    CheckInRecords = _checkInRecords.ToList()
                };

                var json = JsonSerializer.Serialize(archiveData);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", $"archived_event_{eventId}", json);
                
                // Clear current records after archiving
                _checkInRecords.Clear();
                await SaveCheckInRecordsAsync();
                OnPropertyChanged(nameof(CheckInRecords));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error archiving event data: {ex.Message}");
            }
        }

        private async Task SaveCheckInRecordsAsync()
        {
            if (_currentEventId.HasValue)
            {
                try
                {
                    var json = JsonSerializer.Serialize(_checkInRecords);
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", $"checkins_event_{_currentEventId}", json);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving check-in records: {ex.Message}");
                }
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