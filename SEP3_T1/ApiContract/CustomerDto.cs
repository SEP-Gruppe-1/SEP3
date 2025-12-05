namespace ApiContract;

public record CustomerCreateDto(string Name, string Phone, string Email, string Password);

public record CustomerUpdateDto(string Name, string Phone, string Email);

public record CustomerDto(string Name, string Phone, string Email);

public record SaveCustomerDto(string Name, string Phone, string Email, string Password);