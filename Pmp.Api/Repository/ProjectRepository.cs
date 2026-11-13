using Microsoft.EntityFrameworkCore;
using Pmp.Api.Data;
using Pmp.Api.Models;

namespace Pmp.Api.Repositories;

public class ProjectRepository(AppDbContext context) : IProjectRepository
{
    private readonly AppDbContext _context = context;

    public async Task<bool> CodeExistsAsync(string code)
    {
        return await _context.Projects.AnyAsync(project => project.Code == code);
    }

    public async Task<bool> CustomerExistsAsync(int customerId)
    {
        return await _context.Customers.AnyAsync(customer => customer.Id == customerId);
    }

    public async Task<bool> EmployeeExistsAsync(int employeeId)
    {
        return await _context.Employees.AnyAsync(employee => employee.Id == employeeId);
    }

    public async Task<List<Project>> GetAllAsync()
    {
        return await _context.Projects.AsNoTracking().ToListAsync();
    }

    public async Task AddAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();
    }
}
