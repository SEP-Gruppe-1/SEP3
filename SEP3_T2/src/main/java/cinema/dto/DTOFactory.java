package cinema.dto;

import cinema.model.Customer;
import grpccinema.*;

import java.util.ArrayList;
import java.util.List;

public class DTOFactory
{
  public static DTOCustomer createDTOCustomer(Customer customer)
  {
    return DTOCustomer.newBuilder().setName(customer.getName())
        .setPassword(customer.getPassword()).setEmail(customer.getEmail())
        .setPhone(customer.getPhone()).build();
  }

  public static Customer createCustomer(DTOCustomer dtoCustomer)
  {
    return new Customer(dtoCustomer.getName(), dtoCustomer.getPassword(),
        dtoCustomer.getEmail(), dtoCustomer.getPhone());
  }

  public static Customer createCustomer(GetCustomersResponse r)
  {
    return createCustomer(r.getCustomers(0));
  }

  public static Customer[] createCustomers(GetCustomersResponse r)
  {
    Customer[] res = new Customer[r.getCustomersCount()];
    for (int i = 0; i < r.getCustomersCount(); i++)
      res[i] = createCustomer(r.getCustomers(i));
    return res;
  }

  public static GetCustomersRequest createGetCustomersRequest()
  {
    return GetCustomersRequest.newBuilder().build();
  }

  public static GetCustomersResponse createGetCustomersResponse(
      List<Customer> customers)
  {
    ArrayList<DTOCustomer> dtoCustomers = new ArrayList<>();
    for (Customer c : customers)
    {
      dtoCustomers.add(DTOCustomer.newBuilder().setName(c.getName())
          .setPassword(c.getPassword()).setEmail(c.getEmail())
          .setPhone(c.getPhone()).build());
    }
    return GetCustomersResponse.newBuilder().addAllCustomers(dtoCustomers)
        .build();
  }

}
