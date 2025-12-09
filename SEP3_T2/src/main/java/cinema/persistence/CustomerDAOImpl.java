package cinema.persistence;

import cinema.model.Customer;

import java.sql.*;
import java.util.ArrayList;
import java.util.List;

public class CustomerDAOImpl implements CustomerDAO
{
  private static CustomerDAOImpl instance;

  private CustomerDAOImpl() throws SQLException
  {
    DriverManager.registerDriver(new org.postgresql.Driver());
  }

  private static Connection getConnection() throws SQLException
  {
    return DriverManager.getConnection(
        "jdbc:postgresql://localhost:5432/postgres?currentSchema=cinema",
        "postgres", "123");
  }

  public static CustomerDAOImpl getInstance() throws SQLException
  {
    if (instance == null)
    {
      instance = new CustomerDAOImpl();
    }
    return instance;
  }

  @Override public Customer getCustomerByEmail(String email) throws SQLException
  {
    String sql = "SELECT name, password, email, phone, role FROM Customer WHERE email = ?";
    try (Connection conn = getConnection();
        PreparedStatement stmt = conn.prepareStatement(sql))
    {
      stmt.setString(1, email);
      try (ResultSet rs = stmt.executeQuery())
      {
        if (rs.next())
        {
          return new Customer(rs.getString("name"), rs.getString("password"),
              rs.getString("email"), rs.getString("phone"), rs.getString("role"));
        }
      }
    }
    return null;
  }

  @Override public Customer getCustomerByPhone(String phone)
  {
    String sql = "SELECT name, password, role,  email, phone FROM Customer WHERE phone = ?";
    try (Connection conn = getConnection();
        PreparedStatement stmt = conn.prepareStatement(sql))
    {
      stmt.setString(1, phone);
      try (ResultSet rs = stmt.executeQuery())
      {
        if (rs.next())
        {
          return new Customer(rs.getString("name"), rs.getString("password"),
              rs.getString("email"), rs.getString("phone"), rs.getString("role"));
        }
      }
    }
    catch (SQLException e)
    {
      System.out.println(e.toString());
    }
    return null;
  }

  @Override public List<Customer> getAllCustomers()
  {
    List<Customer> customers = new ArrayList<>();
    String sql = "SELECT name, password, email, role, phone FROM Customer";
    try (Connection conn = getConnection();
        PreparedStatement stmt = conn.prepareStatement(sql);
        ResultSet rs = stmt.executeQuery())
    {
      while (rs.next())
      {
        customers.add(
            new Customer(rs.getString("name"), rs.getString("password"),
                rs.getString("email"), rs.getString("phone"), rs.getString("role")));
      }
    }
    catch (SQLException e)
    {
      System.out.println(e.toString());
    }
    return customers;
  }

    @Override
    public void createCustomer(Customer customer) throws SQLException {
        String sql = "INSERT INTO Customer (name, password, email, phone, role) VALUES (?, ?, ?, ?, ?)";
        try (Connection conn = getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql)) {

            stmt.setString(1, customer.getName());
            stmt.setString(2, customer.getPassword());
            stmt.setString(3, customer.getEmail());
            stmt.setString(4, customer.getPhone());  // ✅ PHONE
            stmt.setString(5, customer.getRole());   // ✅ ROLE

            stmt.executeUpdate();
        }
    }


    @Override
    public void updateCustomer(Customer customer) throws SQLException {
        String sql = "UPDATE Customer SET name = ?, password = ?, phone = ?, role = ? WHERE phone = ?";
        try (Connection conn = getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql)) {

            stmt.setString(1, customer.getName());
            stmt.setString(2, customer.getPassword());
            stmt.setString(3, customer.getPhone());
            stmt.setString(4, customer.getRole());   // ✅ ROLE
            stmt.setString(5, customer.getEmail());  // ✅ WHERE email

            stmt.executeUpdate();
        }
    }


    @Override
    public void deleteCustomerByPhone(String phone) throws SQLException {
        String sql = "DELETE FROM cinema.customer WHERE phone = ?";

        try (Connection conn = getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql)) {

            stmt.setString(1, phone);
            stmt.executeUpdate();
        }
    }

}