using Domain;

namespace Services.StudentServices
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAllAsync();
        Task<Student?> GetByIdAsync(int id);
        Task<Student> CreateAsync(Student student);
        Task<bool> UpdateAsync(int id, Student student);
        Task<bool> DeleteAsync(int id);
    }
}
