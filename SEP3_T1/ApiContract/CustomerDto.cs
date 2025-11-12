namespace ApiContract;

public record CustomerCreateDto(string Name, int Phone, string Email);

public record CustomerUpdateDto(int Id, string Name, int Phone, string Email);

public record CustomerDto(int Id, string Name, int Phone, string Email);