package cinema;

import cinema.model.Customer;
import cinema.persistence.CustomerDAO;
import cinema.persistence.CustomerDAOImpl;

public class main
  {
      public static void main(String[] args) {
          try {
              CustomerDAO dao = CustomerDAOImpl.getInstance();

              // Create a new customer
//             Customer newCustomer = new Customer("Rasmus Duus", "HelloMartin1717!", "342727@via.dk", "30439697");
//              dao.createCustomer(newCustomer);
//              System.out.println("Customer created.");

              // Retrieve the customer by email
              Customer retrieved = dao.getCustomerByEmail("342727@via.dk");
              if (retrieved != null) {
                  System.out.println("Customer retrieved: " + retrieved.getName() + ", " + retrieved.getEmail());
              } else {
                  System.out.println("Customer not found.");
              }
          } catch (Exception e) {
              System.out.println("Error: " + e.getMessage());
          }
      }
  }