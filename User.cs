public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    private List<Ticket> ReservedTickets = new();

    public User(int id, string firstName, string lastName, string email, string password, string phoneNumber)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        PhoneNumber = phoneNumber;
        ReservedTickets = TicketHandler.GetTicketsByUser(this);
         
    }
    public List<Ticket> GetTickets() => ReservedTickets;

}
