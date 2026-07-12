using TommyRoom.Api.Helpers;
using TommyRoom.Shared.Entities;
using TommyRoom.Shared.Enums;

namespace TommyRoom.Api.Data;

public class SeedDb(RoomDataContext datacontext, IUserHelper userHelper)
{
    private readonly RoomDataContext _datacontext = datacontext;
    private readonly IUserHelper _userHelper = userHelper;

    public async Task SeedAsync()
    {
        await _datacontext.Database.EnsureCreatedAsync();
        await CheckRolesAsync();
        await CheckSuperAdminAsync("El Tommy", "eltommy@yopmail.com", "322 311 420", "naruto.gif", UserType.SuperAdmin);
        await CheckUserAsync("Naruto", "naruto@yopmail.com", "322 311 460", "https://tse2.mm.bing.net/th/id/OIP.qmzNgDc5Qif3CKQhHPz0CwHaJe?w=1600&h=2048&rs=1&pid=ImgDetMain&o=7&rm=3", UserType.User);
        await CheckUserAsync("Brad Pitt", "brad@yopmail.com", "322 311 462", "https://www.famousbirthdays.com/headshots/brad-pitt-9.jpg", UserType.User);
        await CheckUserAsync("Angelina Jolie", "angelina@yopmail.com", "322 311 620", "https://th.bing.com/th/id/R.ea08e41477d34ca50ea1d471ae9a24c1?rik=syM1YQV3YAeA8A&riu=http%3a%2f%2fwww.pngall.com%2fwp-content%2fuploads%2f4%2fAngelina-Jolie-PNG-Download-Image-180x180.png&ehk=9PpQoKwZZnToSx13c8BKIY7KF8ZVFGienK24OKAGXTk%3d&risl=&pid=ImgRaw&r=0", UserType.User);
        await CheckUserAsync("Bob Marley", "bob@yopmail.com", "322 314 620", "https://th.bing.com/th/id/R.0775505a15a8846b6aa0930ab5e0d8dd?rik=wC1YaVdSPWOamQ&riu=http%3a%2f%2fwww.myfirstrecord.co.uk%2frecordpress%2fwp-content%2fuploads%2f2011%2f05%2fBob-Marley1-150x150.jpg&ehk=nOBcCpQOgc8iSniug5PCieOqW5fNq3ja%2faS%2bCWw9xxE%3d&risl=&pid=ImgRaw&r=0", UserType.User);
        await CheckUserAsync("Mila Azul", "mila@yopmail.com", "382 314 620", "https://image.tmdb.org/t/p/original/rfOcceWpTCZoMYvRiQ8FoTpK59T.jpg", UserType.User);
        await CheckUserAsync("Sai Ambu", "miha@yopmail.com", "377 314 620", "https://tse4.mm.bing.net/th/id/OIP.ZltgcqHOJxCsj2Pf9IhKqQAAAA?rs=1&pid=ImgDetMain&o=7&rm=3", UserType.User);
        await CheckUserAsync("Hynata Hyuga", "hyna@tommy.com", "928 172 126", "https://pt.quizur.com/_image?href=https://img.quizur.com/f/img6149da08ee4b74.87549065.jpg?lastEdited=1632229911&w=600&h=600&f=webp", UserType.User);
        await CheckUserAsync("Ino Sarutobi", "ino@tommy.com", "928 172 129", "https://th.bing.com/th/id/R.cbca06d335b58ddea8eafd6f1207f994?rik=XIcp71ShwjKQ3g&riu=http%3a%2f%2ficons.iconseeker.com%2fpng%2f128%2fnaruto-vol-2%2fyamanaka-ino.png&ehk=m2lPEUWXtlWr9W%2fne%2bmOtCfLTzCrULFN5%2bNL%2b%2fPVciI%3d&risl=&pid=ImgRaw&r=0", UserType.User);
    }

    private async Task CheckRolesAsync()
    {
        await _userHelper.CheckRoleAsync(UserType.SuperAdmin.ToString());
        await _userHelper.CheckRoleAsync(UserType.Guest.ToString());
        await _userHelper.CheckRoleAsync(UserType.Receptionist.ToString());
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

    private async Task<User> CheckUserAsync(string fullName, string email, string phone, string imagePath, UserType userType)
    {
        var user = await _userHelper.GetUserAsync(email);

        if (user == null)
        {
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


}
