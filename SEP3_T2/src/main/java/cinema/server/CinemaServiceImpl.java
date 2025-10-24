package cinema.server;

import cinema.dto.DTOFactory;
import cinema.model.Customer;
import cinema.persistence.CustomerDAO;
import grpccinema.*;
import io.grpc.stub.StreamObserver;

public class CinemaServiceImpl extends CinemaServiceGrpc.CinemaServiceImplBase
{
  private CustomerDAO customerDAO;

  public CinemaServiceImpl(CustomerDAO customerDAO)
  {
    this.customerDAO = customerDAO;
  }

  @Override public void getCustomers(GetCustomersRequest request,
      StreamObserver<GetCustomersResponse> responseObserver)
  {
    GetCustomersResponse response = DTOFactory.createGetCustomersResponse(
        customerDAO.getAllCustomers());

    responseObserver.onNext(response);
    responseObserver.onCompleted();
  }

  @Override public void getCustomerByPhone(GetCustomerByPhoneRequest request,
      StreamObserver<GetCustomerByPhoneResponse> responseObserver)
  {
    Customer customer = customerDAO.getCustomerByPhone(request.getPhone());
    DTOCustomer dtoCustomer = DTOFactory.createDTOCustomer(customer);
    GetCustomerByPhoneResponse response = GetCustomerByPhoneResponse.newBuilder()
        .setCustomer(dtoCustomer).build();

    responseObserver.onNext(response);
    responseObserver.onCompleted();
  }
}
