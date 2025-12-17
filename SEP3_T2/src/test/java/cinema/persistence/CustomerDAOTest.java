package cinema.persistence;

import cinema.model.Customer;
import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mindrot.jbcrypt.BCrypt;

import java.sql.SQLException;
import java.util.List;

import static org.junit.jupiter.api.Assertions.*;

class CustomerDAOTest
{
  private CustomerDAOImpl dao;
  private String testPhone;
  private String testEmail;
  private final String rawPassword = "Passw0rd!";

  @BeforeEach
  void setUp() throws SQLException
  {
    dao = CustomerDAOImpl.getInstance();
    long suffix = System.currentTimeMillis() % 1000000;
    testPhone = "999" + suffix;                // keep <= 15 chars
    testEmail = "test+" + suffix + "@example.com";

    Customer testCustomer = new Customer("Test User", rawPassword, testEmail, testPhone, "Customer");
    dao.createCustomer(testCustomer);
  }

  @AfterEach
  void tearDown() throws SQLException
  {
    // remove the test customer created in setUp
    dao.deleteCustomerByPhone(testPhone);
  }

  @Test
  void testGetCustomerByEmail() throws SQLException
  {
    Customer c = dao.getCustomerByEmail(testEmail);
    assertNotNull(c, "Customer should be found by email");
    assertEquals("Test User", c.getName());
    assertEquals(testEmail, c.getEmail());
    assertEquals(testPhone, c.getPhone());
    assertEquals("Customer", c.getRole());
    // password stored is hashed; verify using BCrypt
    assertTrue(BCrypt.checkpw(rawPassword, c.getPassword()), "Stored password should match raw password when checked with BCrypt");
  }

  @Test
  void testGetCustomerByPhone() throws SQLException
  {
    Customer c = dao.getCustomerByPhone(testPhone);
    assertNotNull(c, "Customer should be found by phone");
    assertEquals(testPhone, c.getPhone());
    assertEquals(testEmail, c.getEmail());
    assertTrue(BCrypt.checkpw(rawPassword, c.getPassword()));
  }

  @Test
  void testGetAllCustomersContainsTestCustomer() throws SQLException
  {
    List<Customer> all = dao.getAllCustomers();
    boolean found = all.stream().anyMatch(c -> testPhone.equals(c.getPhone()));
    assertTrue(found, "getAllCustomers should include the test customer");
  }

  @Test
  void testUpdateCustomerRole() throws SQLException
  {
    // change role to Employee
    dao.updateCustomerRole(testPhone, "Employee");
    Customer updated = dao.getCustomerByPhone(testPhone);
    assertNotNull(updated);
    assertEquals("Employee", updated.getRole());

    // restore role to Customer for cleanliness (optional)
    dao.updateCustomerRole(testPhone, "Customer");
    Customer restored = dao.getCustomerByPhone(testPhone);
    assertEquals("Customer", restored.getRole());
  }

  @Test
  void testCreateAndDeleteCustomer() throws SQLException
  {
    // create a second temporary customer to test create + delete
    String tmpPhone = testPhone + "1";
    String tmpEmail = "tmp+" + tmpPhone + "@example.com";
    Customer tmp = new Customer("Tmp", "tmpPass", tmpEmail, tmpPhone, "Customer");
    dao.createCustomer(tmp);

    Customer found = dao.getCustomerByPhone(tmpPhone);
    assertNotNull(found, "temporarily created customer should exist");

    // delete and assert removal
    dao.deleteCustomerByPhone(tmpPhone);
    Customer afterDelete = dao.getCustomerByPhone(tmpPhone);
    assertNull(afterDelete, "temporary customer should be deleted");
  }
}