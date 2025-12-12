namespace ApiContract;

public record CustomerCreateDto(string Name, string Phone, string Email, string Password, string Role);

public record CustomerUpdateDto(string Name, string Phone, string Email, string Role);

public record CustomerDto(string Name, string Phone, string Email, string Role);

public record SaveCustomerDto(string Name, string Phone, string Email, string Password, string Role);

public record UpdateCustomerRoleDto(string Phone, string NewRole);

public record CustomerBookingDto(int ScreeningId, string MovieTitle, string Date, string Time, List<string> Seats, List<int> SeatIds );

