using Employee_Mangement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Employee_Mangement.Controllers
{
    public class EmpController : Controller
    {
        private readonly IConfiguration _configation;
        public EmpController(IConfiguration configuration) {
            _configation = configuration;
        }
        public IActionResult Index(string search="")
        {
            List<JoinModel> employeeList = new List<JoinModel>();
            string CS = _configation.GetConnectionString("DefaultConnection");
            if (!(string.IsNullOrEmpty(search)))
            {
                
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand(@"SELECT employeeid,employeename,designationname,salary 
                                                 FROM employee ep 
                                                 INNER JOIN designation ds ON ep.designationid=ds.designationid AND ds.flagdeleted=0 
                                                 WHERE ep.flagdelted=0 AND employeename LIKE '"+search+ "%' OR employeename LIKE '%" + search + "%' OR employeename LIKE '" + search + "%'", con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        var employee = new JoinModel();
                        employee.employeeid = Convert.ToInt64(rdr["employeeid"]);
                        employee.employeename = rdr["employeename"].ToString();
                        employee.designationname = rdr["designationname"].ToString();
                        employee.salary = Convert.ToDecimal(rdr["salary"]);
                        employeeList.Add(employee);
                    }
                   
                }
                if (employeeList.Count == 0)
                {
                    ViewBag.Message = "Search Employee Not Found Here";
                    return View(employeeList);
                }
                return View(employeeList);

            }
/*            List<JoinModel> employeeList = new List<JoinModel>();
            string CS = _configation.GetConnectionString("DefaultConnection");*/
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand(@"SELECT employeeid,employeename,designationname,salary 
                                                 FROM employee ep 
                                                 INNER JOIN designation ds ON ep.designationid=ds.designationid AND ds.flagdeleted=0 
                                                 WHERE ep.flagdelted=0", con);
                cmd.CommandType = CommandType.Text;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var employee = new JoinModel();
                    employee.employeeid = Convert.ToInt64(rdr["employeeid"]);
                    employee.employeename = rdr["employeename"].ToString();
                    employee.designationname = rdr["designationname"].ToString();
                    employee.salary = Convert.ToDecimal(rdr["salary"]);
                    employeeList.Add(employee);
                }

                }
                return View(employeeList);
        }
        //GET AddEdit
        public IActionResult AddEdit(int? id) {
            ViewBag.list = new SelectList(Getdesignation(), "designationid", "designationname");
            if (id != null)
            {
                EmployeeModel employeeList = new EmployeeModel();
                string CS = _configation.GetConnectionString("DefaultConnection");
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand(@"SELECT employeeid,employeename,designationid,salary 
                                                 FROM employee
                                                 WHERE flagdelted=0 AND employeeid="+id+"", con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();

                    SqlDataReader rdrs = cmd.ExecuteReader();
                    while (rdrs.Read())
                    {
                        employeeList = new EmployeeModel
                        {
                            employeeid = Convert.ToInt64(rdrs["employeeid"]),
                            employeename = rdrs["employeename"].ToString(),
                            designationid = Convert.ToInt64(rdrs["designationid"]),
                            salary = Convert.ToDecimal(rdrs["salary"])
                        };
                        
                    }

                }
                return View(employeeList);
            }

            return View();

        }
        [HttpPost]
        public IActionResult AddEdit(int? id,EmployeeModel emp)
        {
            ViewBag.list = new SelectList(Getdesignation(), "designationid", "designationname");
            if (id != null) {
                if (emp.employeeid != null && !(String.IsNullOrEmpty(emp.employeename)) && emp.designationid != null && emp.salary != null)
                {
                    string CS = _configation.GetConnectionString("DefaultConnection");
                    using (SqlConnection con = new SqlConnection(CS))
                    {
                        SqlCommand cmd = new SqlCommand(@"UPDATE employee SET employeename='" +emp.employeename+ "',designationid="+emp.designationid+",salary= "+emp.salary+ " WHERE employeeid="+emp.employeeid+"", con);
                        cmd.CommandType = CommandType.Text;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return View(emp);
                }

            }

            if (emp.employeeid!=null && !(String.IsNullOrEmpty(emp.employeename)) && emp.designationid!=null && emp.salary!=null)
            {
                string CS = _configation.GetConnectionString("DefaultConnection");
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand(@"INSERT INTO employee(employeename,designationid,salary)VALUES('" + emp.employeename+"','"+emp.designationid+"',"+emp.salary+")", con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    return RedirectToAction("Index");
                }
        }
            return View(emp);
    }
    private List<Designation> Getdesignation()
        {
            List<Designation> employeeList = new List<Designation>();
            string CS = _configation.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand(@"SELECT designationid,designationname 
                                                 FROM designation
                                                 WHERE flagdeleted=0", con);
                cmd.CommandType = CommandType.Text;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var employee = new Designation();
                    employee.designationid = Convert.ToInt64(rdr["designationid"]);
                    employee.designationname = rdr["designationname"].ToString();
                   
                    employeeList.Add(employee);
                }

            }
            return employeeList;
        }
    public IActionResult Delete(int? id)
        {
            string CS = _configation.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand(@"DELETE FROM employee WHERE employeeid="+id+"", con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                var check=cmd.ExecuteNonQuery();
                return RedirectToAction("Index");
            }

        }
    }
}
