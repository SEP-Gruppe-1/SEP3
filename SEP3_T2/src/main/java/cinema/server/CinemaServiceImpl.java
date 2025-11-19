package cinema.server;

import cinema.dto.DTOFactory;
import cinema.model.Customer;
import cinema.persistence.CustomerDAO;
import grpccinema.*;
import io.grpc.stub.StreamObserver;

import java.sql.SQLException;

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

  @Override public void saveCustomer(SaveCustomerRequest request,
      StreamObserver<SaveCustomerResponse> responseObserver)  {
      try {
          // DTO fra request
          DTOCustomer dto = request.getCustomer();

          // Konverter DTO → domain model
          Customer customer = DTOFactory.createCustomer(dto);

          // Tjek om kunden allerede findes (via phone fx.)
          Customer existing = customerDAO.getCustomerByPhone(customer.getPhone());

          if (existing == null) {
              // Ny kunde → create
              customerDAO.createCustomer(customer);
          } else {
              // Eksisterende kunde → update
              customerDAO.updateCustomer(customer);
          }

          // Konverter til DTO for response
          DTOCustomer savedDto = DTOFactory.createDTOCustomer(customer);

          SaveCustomerResponse response = SaveCustomerResponse.newBuilder()
                  .setCustomer(savedDto)
                  .build();

          responseObserver.onNext(response);
          responseObserver.onCompleted();

      } catch (Exception e) {
          e.printStackTrace();
          responseObserver.onError(
                  io.grpc.Status.INTERNAL
                          .withDescription("Could not save customer: " + e.getMessage())
                          .asRuntimeException()
          );
      }
  }
}
