namespace ApiContract;

public record CustomerCreateDto(string Name, int Phone, string Email, string Password);

public record CustomerUpdateDto(string Name, int Phone, string Email);

public record CustomerDto(string Name, int Phone, string Email);

public record SaveCustomerDto(string Name, int Phone, string Email, string Password);