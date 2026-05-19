using AirportFlightManagement.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AirportFlightManagement.Services;

public class DatabaseService
{
    private static DatabaseService? _instance;
    public static DatabaseService Instance => _instance ??= new DatabaseService();

    private const string ConnectionString = "Host=localhost;Port=5432;Database=airport_flight_management;Username=postgres;Password=postgres;SSL Mode=Disable;";

    public string? LastError { get; private set; }

    public ObservableCollection<Flight> Flights { get; } = new();
    public ObservableCollection<ArchivedFlight> ArchivedFlights { get; } = new();
    public ObservableCollection<Plane> Planes { get; } = new();
    public ObservableCollection<Airport> Airports { get; } = new();
    public ObservableCollection<User> Users { get; } = new();

    public DatabaseService()
    {
        LoadDataFromDatabase();
    }

    private void LoadDataFromDatabase()
    {
        TryLoad("Airports", LoadAirports);
        TryLoad("Planes", LoadPlanes);
        TryLoad("Users", LoadUsers);
        TryLoad("Flights", LoadFlights);
        TryLoad("ArchivedFlights", LoadArchivedFlights);
    }

    private void TryLoad(string section, Action loader)
    {
        try
        {
            loader();
        }
        catch (Exception ex)
        {
            LastError = $"{section}: {ex.Message}";
            System.Diagnostics.Debug.WriteLine($"Load error [{section}]: {ex.Message}");
        }
    }

    private void LoadAirports()
    {
        Airports.Clear();
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            connection.Open();
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT id, name, code, city, country, is_active FROM airports ORDER BY name";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Airports.Add(new Airport
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Code = reader.GetString(2),
                            City = reader.GetString(3),
                            Country = reader.GetString(4),
                            IsActive = reader.GetBoolean(5)
                        });
                    }
                }
            }
        }
    }

    private void LoadPlanes()
    {
        Planes.Clear();
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            connection.Open();
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT id, registration_number, model, at_airport FROM planes ORDER BY id";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Planes.Add(new Plane
                        {
                            Id = reader.GetInt32(0),
                            RegistrationNumber = reader.GetString(1),
                            Model = reader.IsDBNull(2) ? null : reader.GetString(2),
                            AtAirport = reader.GetBoolean(3)
                        });
                    }
                }
            }
        }
    }

    private void LoadUsers()
    {
        Users.Clear();
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            connection.Open();
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT id, username, password_hash, role, last_login FROM users ORDER BY id";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Users.Add(new User
                        {
                            Id = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            PasswordHash = reader.GetString(2),
                            Role = reader.GetString(3),
                            LastLogin = reader.IsDBNull(4) ? null : reader.GetDateTime(4)
                        });
                    }
                }
            }
        }
    }

    private void LoadFlights()
    {
        Flights.Clear();
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            connection.Open();
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT f.id, f.flight_code, f.departure_time, f.arrival_time, f.plane_id, f.total_seats, f.available_seats, f.ticket_price, f.status, f.departure_date, f.departure_airport_id, a.id, a.name, a.code, f.destination_airport_id, da.id, da.name, da.code FROM flights f LEFT JOIN airports a ON f.departure_airport_id = a.id LEFT JOIN airports da ON f.destination_airport_id = da.id ORDER BY f.departure_date, f.departure_time";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var flight = new Flight
                        {
                            Id = reader.GetInt32(0),
                            FlightCode = reader.GetString(1),
                            DepartureTime = reader.GetFieldValue<TimeOnly>(2),
                            ArrivalTime = reader.GetFieldValue<TimeOnly>(3),
                            PlaneId = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                            TotalSeats = reader.GetInt32(5),
                            AvailableSeats = reader.GetInt32(6),
                            TicketPrice = reader.GetDecimal(7),
                            Status = reader.GetString(8),
                            DepartureDate = reader.GetFieldValue<DateOnly>(9),
                            DepartureAirportId = reader.IsDBNull(10) ? null : reader.GetInt32(10),
                            DestinationAirportId = reader.IsDBNull(14) ? null : reader.GetInt32(14)
                        };

                        if (!reader.IsDBNull(11))
                        {
                            flight.DepartureAirport = new Airport
                            {
                                Id = reader.GetInt32(11),
                                Name = reader.GetString(12),
                                Code = reader.GetString(13)
                            };
                        }

                        if (!reader.IsDBNull(15))
                        {
                            flight.DestinationAirport = new Airport
                            {
                                Id = reader.GetInt32(15),
                                Name = reader.GetString(16),
                                Code = reader.GetString(17)
                            };
                        }

                        Flights.Add(flight);
                    }
                }
            }
        }
    }
    
    private void LoadArchivedFlights()
    {
        ArchivedFlights.Clear();
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            connection.Open();
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT id, flight_code, destination, departure_time, arrival_time, plane_id, total_seats, available_seats, ticket_price, day_of_week, canceled_at, cancellation_reason, departure_airport_id, destination_airport_id FROM archived_flights ORDER BY canceled_at DESC";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ArchivedFlights.Add(new ArchivedFlight
                        {
                            Id = reader.GetInt32(0),
                            FlightCode = reader.GetString(1),
                            Destination = reader.GetString(2),
                            DepartureTime = reader.GetFieldValue<TimeOnly>(3),
                            ArrivalTime = reader.GetFieldValue<TimeOnly>(4),
                            PlaneId = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                            TotalSeats = reader.GetInt32(6),
                            AvailableSeats = reader.GetInt32(7),
                            TicketPrice = reader.GetDecimal(8),
                            DayOfWeek = reader.GetInt16(9),
                            CanceledAt = reader.GetDateTime(10),
                            CancellationReason = reader.IsDBNull(11) ? null : reader.GetString(11),
                            DepartureAirportId = reader.IsDBNull(12) ? null : reader.GetInt32(12),
                            DestinationAirportId = reader.IsDBNull(13) ? null : reader.GetInt32(13)
                        });
                    }
                }
            }
        }
    }

    public void AddFlight(Flight flight)
    {
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            connection.Open();
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO flights (flight_code, departure_time, arrival_time, plane_id, total_seats, available_seats, ticket_price, status, departure_date, departure_airport_id, destination_airport_id) VALUES (@flightCode, @departureTime, @arrivalTime, @planeId, @totalSeats, @availableSeats, @ticketPrice, @status, @departureDate, @departureAirportId, @destinationAirportId) RETURNING id";
                cmd.Parameters.AddWithValue("@flightCode", flight.FlightCode);
                cmd.Parameters.AddWithValue("@departureTime", flight.DepartureTime);
                cmd.Parameters.AddWithValue("@arrivalTime", flight.ArrivalTime);
                cmd.Parameters.AddWithValue("@planeId", flight.PlaneId > 0 ? (object)flight.PlaneId : DBNull.Value);
                cmd.Parameters.AddWithValue("@totalSeats", flight.TotalSeats);
                cmd.Parameters.AddWithValue("@availableSeats", flight.AvailableSeats);
                cmd.Parameters.AddWithValue("@ticketPrice", flight.TicketPrice);
                cmd.Parameters.AddWithValue("@status", flight.Status ?? "On Time");
                cmd.Parameters.AddWithValue("@departureDate", flight.DepartureDate);
                cmd.Parameters.AddWithValue("@departureAirportId", flight.DepartureAirportId.HasValue ? (object)flight.DepartureAirportId.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@destinationAirportId", flight.DestinationAirportId.HasValue ? (object)flight.DestinationAirportId.Value : DBNull.Value);
                flight.Id = (int)cmd.ExecuteScalar()!;
            }
        }
        Flights.Add(flight);
    }

    public void UpdateFlight(Flight flight)
    {
        var existing = Flights.FirstOrDefault(f => f.Id == flight.Id);
        if (existing != null)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "UPDATE flights SET flight_code = @flightCode, departure_time = @departureTime, arrival_time = @arrivalTime, plane_id = @planeId, total_seats = @totalSeats, available_seats = @availableSeats, ticket_price = @ticketPrice, status = @status, departure_date = @departureDate, departure_airport_id = @departureAirportId, destination_airport_id = @destinationAirportId WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", flight.Id);
                    cmd.Parameters.AddWithValue("@flightCode", flight.FlightCode);
                    cmd.Parameters.AddWithValue("@departureTime", flight.DepartureTime);
                    cmd.Parameters.AddWithValue("@arrivalTime", flight.ArrivalTime);
                    cmd.Parameters.AddWithValue("@planeId", flight.PlaneId > 0 ? (object)flight.PlaneId : DBNull.Value);
                    cmd.Parameters.AddWithValue("@totalSeats", flight.TotalSeats);
                    cmd.Parameters.AddWithValue("@availableSeats", flight.AvailableSeats);
                    cmd.Parameters.AddWithValue("@ticketPrice", flight.TicketPrice);
                    cmd.Parameters.AddWithValue("@status", flight.Status ?? "On Time");
                    cmd.Parameters.AddWithValue("@departureDate", flight.DepartureDate);
                    cmd.Parameters.AddWithValue("@departureAirportId", flight.DepartureAirportId.HasValue ? (object)flight.DepartureAirportId.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@destinationAirportId", flight.DestinationAirportId.HasValue ? (object)flight.DestinationAirportId.Value : DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
            existing.FlightCode = flight.FlightCode;
            existing.DepartureTime = flight.DepartureTime;
            existing.ArrivalTime = flight.ArrivalTime;
            existing.PlaneId = flight.PlaneId;
            existing.DepartureAirportId = flight.DepartureAirportId;
            existing.DepartureAirport = flight.DepartureAirport;
            existing.DestinationAirportId = flight.DestinationAirportId;
            existing.DestinationAirport = flight.DestinationAirport;
            existing.TotalSeats = flight.TotalSeats;
            existing.AvailableSeats = flight.AvailableSeats;
            existing.TicketPrice = flight.TicketPrice;
            existing.Status = flight.Status ?? "On Time";
            existing.DepartureDate = flight.DepartureDate;
        }
    }

    public void DeleteFlight(int flightId, string reason = "User canceled")
    {
        var flight = Flights.FirstOrDefault(f => f.Id == flightId);
        if (flight != null)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO archived_flights (flight_code, destination, departure_time, arrival_time, plane_id, total_seats, available_seats, ticket_price, day_of_week, departure_airport_id, destination_airport_id, cancellation_reason) VALUES (@flightCode, @destination, @departureTime, @arrivalTime, @planeId, @totalSeats, @availableSeats, @ticketPrice, @dayOfWeek, @departureAirportId, @destinationAirportId, @reason)";
                    cmd.Parameters.AddWithValue("@flightCode", flight.FlightCode);
                    cmd.Parameters.AddWithValue("@destination", flight.Destination);
                    cmd.Parameters.AddWithValue("@departureTime", flight.DepartureTime);
                    cmd.Parameters.AddWithValue("@arrivalTime", flight.ArrivalTime);
                    cmd.Parameters.AddWithValue("@planeId", flight.PlaneId > 0 ? (object)flight.PlaneId : DBNull.Value);
                    cmd.Parameters.AddWithValue("@totalSeats", flight.TotalSeats);
                    cmd.Parameters.AddWithValue("@availableSeats", flight.AvailableSeats);
                    cmd.Parameters.AddWithValue("@ticketPrice", flight.TicketPrice);
                    cmd.Parameters.AddWithValue("@dayOfWeek", (int)flight.DepartureDate.DayOfWeek);
                    cmd.Parameters.AddWithValue("@departureAirportId", flight.DepartureAirportId.HasValue ? (object)flight.DepartureAirportId.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@destinationAirportId", flight.DestinationAirportId.HasValue ? (object)flight.DestinationAirportId.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@reason", reason);
                    cmd.ExecuteNonQuery();
                }
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM flights WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", flightId);
                    cmd.ExecuteNonQuery();
                }
            }
            Flights.Remove(flight);
            LoadArchivedFlights();
        }
    }

    public void RefreshFlights() => TryLoad("Flights", LoadFlights);
    public void RefreshPlanes() => TryLoad("Planes", LoadPlanes);
    public void RefreshAirports() => TryLoad("Airports", LoadAirports);
    public void RefreshArchivedFlights() => TryLoad("ArchivedFlights", LoadArchivedFlights);
    public void RefreshAll() => LoadDataFromDatabase();

    public int GetTotalFlights() => Flights.Count;
    public int GetDeparturesCount() => Flights.Count;
    public int GetPlanesAtAirportCount() => Planes.Count(p => p.AtAirport);
    public int GetTotalAvailableSeats() => Flights.Sum(f => f.AvailableSeats);
    public IEnumerable<Flight> GetFlightsByDestination(string destination) => Flights.Where(f => f.Destination.Equals(destination, StringComparison.OrdinalIgnoreCase)).OrderBy(f => f.DepartureDate);
    public IEnumerable<Flight> GetFlightsByTimeRange(string destination, TimeOnly fromTime, TimeOnly toTime) => Flights.Where(f => f.Destination.Equals(destination, StringComparison.OrdinalIgnoreCase) && f.DepartureTime >= fromTime && f.DepartureTime <= toTime).OrderBy(f => f.DepartureDate);
    public Flight? GetLongestFlight() => Flights.OrderByDescending(f => f.Duration).FirstOrDefault();
    public decimal GetAverageTicketPrice(string destination) => Flights.Where(f => f.Destination.Equals(destination, StringComparison.OrdinalIgnoreCase)).Average(f => f.TicketPrice);
    public IEnumerable<Flight> GetFlightsByDay(int dayOfWeek) => Flights.Where(f => (int)f.DepartureDate.DayOfWeek == dayOfWeek);
    public int GetAvailableSeatsForFlight(int flightId) => Flights.FirstOrDefault(f => f.Id == flightId)?.AvailableSeats ?? 0;
    public int GetCanceledFlightsCount() => ArchivedFlights.Count;

    public User? GetUserByUsername(string username) => Users.FirstOrDefault(u => u.Username == username);

    public void UpdateLastLogin(int userId)
    {
        var user = Users.FirstOrDefault(u => u.Id == userId);
        if (user != null)
        {
            var now = DateTime.UtcNow;
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "UPDATE users SET last_login = @lastLogin WHERE id = @id";
                    cmd.Parameters.AddWithValue("@lastLogin", now);
                    cmd.Parameters.AddWithValue("@id", userId);
                    cmd.ExecuteNonQuery();
                }
            }
            user.LastLogin = now;
        }
    }

    // Plane Operations
    public void AddPlane(Plane plane)
    {
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            connection.Open();
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO planes (registration_number, model, at_airport) VALUES (@registrationNumber, @model, @atAirport) RETURNING id";
                cmd.Parameters.AddWithValue("@registrationNumber", plane.RegistrationNumber);
                cmd.Parameters.AddWithValue("@model", plane.Model ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@atAirport", plane.AtAirport);
                plane.Id = (int)cmd.ExecuteScalar()!;
            }
        }
        Planes.Add(plane);
    }

    public void UpdatePlane(Plane plane)
    {
        var existing = Planes.FirstOrDefault(p => p.Id == plane.Id);
        if (existing != null)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "UPDATE planes SET registration_number = @registrationNumber, model = @model, at_airport = @atAirport WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", plane.Id);
                    cmd.Parameters.AddWithValue("@registrationNumber", plane.RegistrationNumber);
                    cmd.Parameters.AddWithValue("@model", plane.Model ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@atAirport", plane.AtAirport);
                    cmd.ExecuteNonQuery();
                }
            }
            existing.RegistrationNumber = plane.RegistrationNumber;
            existing.Model = plane.Model;
            existing.AtAirport = plane.AtAirport;
        }
    }

    public void DeletePlane(int planeId)
    {
        var plane = Planes.FirstOrDefault(p => p.Id == planeId);
        if (plane != null)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM planes WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", planeId);
                    cmd.ExecuteNonQuery();
                }
            }
            Planes.Remove(plane);
        }
    }

    // Airport Operations
    public List<Airport> GetAirports() => Airports.ToList();

    public void AddAirport(Airport airport)
    {
        using (var connection = new NpgsqlConnection(ConnectionString))
        {
            connection.Open();
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO airports (name, code, city, country, is_active) VALUES (@name, @code, @city, @country, @isActive) RETURNING id";
                cmd.Parameters.AddWithValue("@name", airport.Name);
                cmd.Parameters.AddWithValue("@code", airport.Code);
                cmd.Parameters.AddWithValue("@city", airport.City);
                cmd.Parameters.AddWithValue("@country", airport.Country);
                cmd.Parameters.AddWithValue("@isActive", airport.IsActive);
                airport.Id = (int)cmd.ExecuteScalar()!;
            }
        }
        Airports.Add(airport);
    }

    public void UpdateAirport(Airport airport)
    {
        var existing = Airports.FirstOrDefault(a => a.Id == airport.Id);
        if (existing != null)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "UPDATE airports SET name = @name, code = @code, city = @city, country = @country, is_active = @isActive WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", airport.Id);
                    cmd.Parameters.AddWithValue("@name", airport.Name);
                    cmd.Parameters.AddWithValue("@code", airport.Code);
                    cmd.Parameters.AddWithValue("@city", airport.City);
                    cmd.Parameters.AddWithValue("@country", airport.Country);
                    cmd.Parameters.AddWithValue("@isActive", airport.IsActive);
                    cmd.ExecuteNonQuery();
                }
            }
            existing.Name = airport.Name;
            existing.Code = airport.Code;
            existing.City = airport.City;
            existing.Country = airport.Country;
            existing.IsActive = airport.IsActive;
        }
    }

    public void DeleteAirport(int airportId)
    {
        var airport = Airports.FirstOrDefault(a => a.Id == airportId);
        if (airport != null)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM airports WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", airportId);
                    cmd.ExecuteNonQuery();
                }
            }
            Airports.Remove(airport);
        }
    }
}
