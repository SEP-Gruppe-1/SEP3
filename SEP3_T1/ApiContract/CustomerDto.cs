namespace ApiContract;

public record CustomerCreateDto(string Name, int Phone, string Email, string Password, string Role);

public record CustomerUpdateDto(string Name, int Phone, string Email, string Role);

public record CustomerDto(string Name, int Phone, string Email, string Role);

public record SaveCustomerDto(string Name, int Phone, string Email, string Password, string Role);
public record UpdateCustomerRoleDto(int Phone, string NewRole);