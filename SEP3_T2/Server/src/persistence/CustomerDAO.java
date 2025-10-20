package persistence;

import model.Customer;

import java.sql.SQLException;

public interface CustomerDAO {
    Customer getCustomerByEmail(String email) throws SQLException;
    Customer getCustomerByPhone(String phone) throws SQLException;
    java.util.List<Customer> getAllCustomers() throws SQLException;
    void createCustomer(Customer customer) throws SQLException;
    void updateCustomer(Customer customer) throws SQLException;
    void deleteCustomer(String email) throws SQLException;
}
