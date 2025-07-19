using TommyRoom.Api.Helpers;
using TommyRoom.Shared.Entities;
using TommyRoom.Shared.Enums;

namespace TommyRoom.Api.Data;

public class SeedDb(DataContext datacontext, IUserHelper userHelper)
{
    private readonly DataContext _datacontext = datacontext;
    private readonly IUserHelper _userHelper = userHelper;

    public async Task SeedAsync()
    {
        await _datacontext.Database.EnsureCreatedAsync();
        await CheckRolesAsync();
        await CheckSuperAdminAsync("El Tommy Gonzales", "eltommy@yopmail.com", "322 311 420", "naruto.gif", UserType.SuperAdmin);
        await CheckUserAsync("Ledys Bedoya", "ledys@yopmail.com", "322 311 460", "NoUser.png", UserType.User);
        await CheckUserAsync("Brad Pitt", "brad@yopmail.com", "322 311 462", "NoUser.png", UserType.User);
        await CheckUserAsync("Angelina Jolie", "angelina@yopmail.com", "322 311 620", "NoUser.png", UserType.User);
        await CheckUserAsync("Bob Marley", "bob@yopmail.com", "322 314 620", "NoUser.png", UserType.User);
        await CheckUserAsync("Nancy Ace", "Nancy@yopmail.com", "392 314 620", "NoUser.png", UserType.User);
        await CheckUserAsync("Mila Azul", "mila@yopmail.com", "382 314 620", "NoUser.png", UserType.User);
        await CheckUserAsync("Miha", "miha@yopmail.com", "377 314 620", "NoUser.png", UserType.User);
        await CheckUserAsync("Tommy Zama", "hokage@tommy.com", "928 172 124", "NoUser.png", UserType.User);
        await CheckUserAsync("Hynata Hyuga", "hyna@tommy.com", "928 172 126", "NoUser.png", UserType.User);
        await CheckUserAsync("Ino Sarutobi", "ino@tommy.com", "928 172 129", "NoUser.png", UserType.User);
        await CheckRoomsAsync();
    }

    private async Task CheckRolesAsync()
    {
        await _userHelper.CheckRoleAsync(UserType.SuperAdmin.ToString());
        await _userHelper.CheckRoleAsync(UserType.User.ToString());
    }

    private async Task<User> CheckSuperAdminAsync(string fullName, string email, string phone, string imageName, UserType userType)
    {
        var user = await _userHelper.GetUserAsync(email);

        if (user == null)
        {
            string imagePath = $"https://schoolbook2024.blob.core.windows.net/users/{imageName}";

            user = new User
            {
                FullName = fullName,
                Email = email,
                UserName = email,
                PhoneNumber = phone,
                Photo = imagePath,
                UserType = userType,
            };

            await _userHelper.AddUserAsync(user, "123456");
            await _userHelper.AddUserToRoleAsync(user, userType.ToString());
        }

        return user;
    }

    private async Task<User> CheckUserAsync(string fullName, string email, string phone, string imageName, UserType userType)
    {
        var user = await _userHelper.GetUserAsync(email);

        if (user == null)
        {
            string imagePath = $"https://schoolbook2024.blob.core.windows.net/default/{imageName}";

            user = new User
            {
                FullName = fullName,
                Email = email,
                UserName = email,
                PhoneNumber = phone,
                Photo = imagePath,
                UserType = userType,
            };

            await _userHelper.AddUserAsync(user, "123456");
            await _userHelper.AddUserToRoleAsync(user, userType.ToString());
        }

        return user;
    }


    private async Task CheckRoomsAsync()
    {
        User? user1 = _datacontext.Users.FirstOrDefault(u => u.FullName == "Brad Pitt");
        User? user2 = _datacontext.Users.FirstOrDefault(u => u.FullName == "Angelina Jolie");
        User? user3 = _datacontext.Users.FirstOrDefault(u => u.FullName == "Mila Azul");

        if (!_datacontext.Rooms.Any())
        {
            _datacontext.Rooms.Add(new() { OwnerId = user1!.Id, Name = "Sala de Juntas", Description = "Sala de juntas principal del edificio.", PricePerNight = 120.00m, Location = "Miraflores, Lima", Capacity = 10, Photo = "https://tse4.mm.bing.net/th/id/OIP.DQDYv5PmsVNIcfKaOci57QHaHa?rs=1&pid=ImgDetMain&o=7&rm=3", IsAvailable = true, });
            _datacontext.Rooms.Add(new() { OwnerId = user1!.Id, Name = "Ocean View Room", Description = "Sala de juntas principal del edificio.", PricePerNight = 225.00m, Location = "San Isidro, Lima", Capacity = 10, Photo = "https://th.bing.com/th/id/OIP.46lL6EcMcWm_KREifyWGUQHaE8?w=259&h=180&c=7&r=0&o=7&pid=1.7&rm=3", IsAvailable = true, });
            _datacontext.Rooms.Add(new() { OwnerId = user2!.Id, Name = "City Loft", Description = "Modern loft in the heart of the city.", PricePerNight = 200.00m, Location = "La Molina, Lima", Capacity = 10, Photo = "https://th.bing.com/th/id/OIP.-3euBm1sKd1ef1jnURQGIwHaEr?w=262&h=180&c=7&r=0&o=7&pid=1.7&rm=3", IsAvailable = true, });
            _datacontext.Rooms.Add(new() { OwnerId = user3!.Id, Name = "Nidito de Amor", Description = "Modern loft in the heart of the city.", PricePerNight = 55.00m, Location = "La Molina, Lima", Capacity = 10, Photo = "https://dbdzm869oupei.cloudfront.net/img/sticker/large/1736.jpg", IsAvailable = false, });

            await _datacontext.SaveChangesAsync();
        }
    }
}
