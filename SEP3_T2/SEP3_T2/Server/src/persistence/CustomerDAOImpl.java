package persistence;

import model.Customer;

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
    String sql = "SELECT name, password, email, phone FROM Customer WHERE email = ?";
    try (Connection conn = getConnection();
        PreparedStatement stmt = conn.prepareStatement(sql))
    {
      stmt.setString(1, email);
      try (ResultSet rs = stmt.executeQuery())
      {
        if (rs.next())
        {
          return new Customer(rs.getString("name"), rs.getString("password"),
              rs.getString("email"), rs.getString("phone"));
        }
      }
    }
    return null;
  }

  @Override public Customer getCustomerByPhone(String phone) throws SQLException
  {
    String sql = "SELECT name, password, email, phone FROM Customer WHERE phone = ?";
    try (Connection conn = getConnection();
        PreparedStatement stmt = conn.prepareStatement(sql))
    {
      stmt.setString(1, phone);
      try (ResultSet rs = stmt.executeQuery())
      {
        if (rs.next())
        {
          return new Customer(rs.getString("name"), rs.getString("password"),
              rs.getString("email"), rs.getString("phone"));
        }
      }
    }
    return null;
  }

  @Override public List<Customer> getAllCustomers() throws SQLException
  {
    List<Customer> customers = new ArrayList<>();
    String sql = "SELECT name, password, email, phone FROM Customer";
    try (Connection conn = getConnection();
        PreparedStatement stmt = conn.prepareStatement(sql);
        ResultSet rs = stmt.executeQuery())
    {
      while (rs.next())
      {
        customers.add(
            new Customer(rs.getString("name"), rs.getString("password"),
                rs.getString("email"), rs.getString("phone")));
      }
    }
    return customers;
  }

  @Override
  public void createCustomer(Customer customer) throws SQLException {
    String sql = "INSERT INTO Customer (name, password, email, phone) VALUES (?, ?, ?, ?)";
    try (Connection conn = getConnection();
        PreparedStatement stmt = conn.prepareStatement(sql)) {
      stmt.setString(1, customer.getName());
      stmt.setString(2, customer.getPassword());
      stmt.setString(3, customer.getEmail());
      stmt.setString(4, customer.getPhone());
      stmt.executeUpdate();
    }
  }

  @Override
  public void updateCustomer(Customer customer) throws SQLException {
    String sql = "UPDATE Customer SET name = ?, password = ?, phone = ? WHERE email = ?";
    try (Connection conn = getConnection();
        PreparedStatement stmt = conn.prepareStatement(sql)) {
      stmt.setString(1, customer.getName());
      stmt.setString(2, customer.getPassword());
      stmt.setString(3, customer.getPhone());
      stmt.setString(4, customer.getEmail());
      stmt.executeUpdate();
    }
  }

  @Override
  public void deleteCustomer(String email) throws SQLException {
    String sql = "DELETE FROM Customer WHERE email = ?";
    try (Connection conn = getConnection();
        PreparedStatement stmt = conn.prepareStatement(sql)) {
      stmt.setString(1, email);
      stmt.executeUpdate();
    }
  }
}