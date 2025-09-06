using System.ComponentModel;
using System.Text.Json;
using Microsoft.JSInterop;

namespace WinstonCheckIn.Services
{
    public class EventDataService : INotifyPropertyChanged
    {
        private readonly IJSRuntime _jsRuntime;
        private List<Event> _events = new();
        private Event? _currentEvent;

        public event PropertyChangedEventHandler? PropertyChanged;

        public IReadOnlyList<Event> Events => _events.AsReadOnly();
        public Event? CurrentEvent => _currentEvent;

        public EventDataService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task LoadEventsAsync()
        {
            try
            {
                var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "gym_events");
                if (!string.IsNullOrEmpty(json))
                {
                    _events = JsonSerializer.Deserialize<List<Event>>(json) ?? new List<Event>();
                }
                else
                {
                    _events = new List<Event>();
                }

                // Set current event (today's event or next upcoming event)
                _currentEvent = _events.FirstOrDefault(e => e.EventDate.Date == DateTime.Today) 
                              ?? _events.FirstOrDefault(e => e.EventDate.Date > DateTime.Today);
                
                OnPropertyChanged(nameof(Events));
                OnPropertyChanged(nameof(CurrentEvent));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading events: {ex.Message}");
                _events = new List<Event>();
            }
        }

        public async Task SaveEventsAsync()
        {
            try
            {
                var json = JsonSerializer.Serialize(_events);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "gym_events", json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving events: {ex.Message}");
            }
        }

        public Task<Event?> GetEventByIdAsync(int id)
        {
            return Task.FromResult(_events.FirstOrDefault(e => e.Id == id));
        }

        public async Task<Event> CreateEventAsync(Event eventModel)
        {
            eventModel.Id = _events.Count > 0 ? _events.Max(e => e.Id) + 1 : 1;
            _events.Add(eventModel);
            await SaveEventsAsync();
            await LoadEventsAsync();
            return eventModel;
        }

        public async Task UpdateEventAsync(Event eventModel)
        {
            var index = _events.FindIndex(e => e.Id == eventModel.Id);
            if (index >= 0)
            {
                _events[index] = eventModel;
                await SaveEventsAsync();
                await LoadEventsAsync();
            }
        }

        public async Task DeleteEventAsync(int id)
        {
            _events.RemoveAll(e => e.Id == id);
            await SaveEventsAsync();
            await LoadEventsAsync();
        }

        public async Task EnableCheckInAsync(int eventId, DateTime? checkInStartTime = null)
        {
            var eventModel = _events.FirstOrDefault(e => e.Id == eventId);
            if (eventModel != null)
            {
                eventModel.IsCheckInEnabled = true;
                eventModel.CheckInStartTime = checkInStartTime ?? DateTime.Now;
                eventModel.UpdatedAt = DateTime.Now;
                await SaveEventsAsync();
                await LoadEventsAsync();
            }
        }

        public async Task DisableCheckInAsync(int eventId)
        {
            var eventModel = _events.FirstOrDefault(e => e.Id == eventId);
            if (eventModel != null)
            {
                eventModel.IsCheckInEnabled = false;
                eventModel.UpdatedAt = DateTime.Now;
                await SaveEventsAsync();
                await LoadEventsAsync();
            }
        }

        public async Task<List<Event>> GenerateWeeklyEventsAsync(DateTime startDate, DateTime endDate)
        {
            var newEvents = new List<Event>();
            var currentDate = startDate;

            while (currentDate <= endDate)
            {
                // Check if it's a Tuesday
                if (currentDate.DayOfWeek == DayOfWeek.Tuesday)
                {
                    var eventName = $"Gym Night - {currentDate:MMM dd, yyyy}";
                    
                    // Check if event already exists
                    var existingEvent = _events.FirstOrDefault(e => e.EventDate.Date == currentDate.Date);
                    
                    if (existingEvent == null)
                    {
                        var newEvent = new Event
                        {
                            Name = eventName,
                            EventDate = currentDate,
                            Description = $"Weekly gym night event on {currentDate:dddd, MMMM dd, yyyy}",
                            IsCheckInEnabled = false,
                            MaxParticipants = 28
                        };
                        
                        newEvents.Add(newEvent);
                    }
                }
                
                currentDate = currentDate.AddDays(1);
            }

            if (newEvents.Any())
            {
                foreach (var newEvent in newEvents)
                {
                    newEvent.Id = _events.Count > 0 ? _events.Max(e => e.Id) + 1 : 1;
                    _events.Add(newEvent);
                }
                
                await SaveEventsAsync();
                await LoadEventsAsync();
            }

            return newEvents;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsCheckInEnabled { get; set; } = false;
        public DateTime? CheckInStartTime { get; set; }
        public int MaxParticipants { get; set; } = 28;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
