package cinema.persistence;

import cinema.model.Customer;

import java.sql.SQLException;

public interface CustomerDAO {
    Customer getCustomerByEmail(String email) throws SQLException;
    Customer getCustomerByPhone(int phone);
    java.util.List<Customer> getAllCustomers();
    void createCustomer(Customer customer) throws SQLException;
    void updateCustomer(Customer customer) throws SQLException;
    void deleteCustomer(String email) throws SQLException;
}
