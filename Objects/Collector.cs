using System.Collections.Generic;
using System;
using System.Data.SqlClient;

namespace CollectorNS
{
  public class Collector
  {
    private int _id;
    private string _description;
    private int _categoryId;
    private DateTime? _dueDate;
    // DateTime dummyDate = new DateTime(2099, 12, 31)

    public Collector(string Description, int CategoryId, DateTime? DueDate = null, int Id = 0)
    {
      _id = Id;
      _description = Description;
      _categoryId = CategoryId;
      _dueDate = DueDate;

      if (_dueDate == null)
      {
        _dueDate = new DateTime(2099, 12, 31);
      }

    }

    public int GetId()
    {
      return _id;
    }
    public string GetDescription()
    {
      return _description;
    }
    public void SetDescription(string newDescription)
    {
      _description = newDescription;
    }
    public int GetCategoryId()
    {
      return _categoryId;
    }
    public void SetCategoryId(int newCategoryId)
    {
      _categoryId = newCategoryId;
    }
    public DateTime? GetDueDate()
    {
      return _dueDate;
    }
    public void SetDueDate(DateTime? newDueDate)
    {
      _dueDate = newDueDate;
    }

    public override bool Equals(System.Object otherCollector)
    {
      if (!(otherCollector is Collector))
      {
        return false;
      }
      else
      {
        Collector newCollector = (Collector) otherCollector;
        bool idEquality = (this.GetId() == newCollector.GetId());
        bool descriptionEquality = (this.GetDescription() == newCollector.GetDescription());
        bool dateTimeEquality = (this.GetDueDate() == newCollector.GetDueDate());
        bool categoryEquality = this.GetCategoryId() == newCollector.GetCategoryId();
        return (idEquality && descriptionEquality && categoryEquality);
      }
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO things (description, due_date, category_id) OUTPUT INSERTED.id VALUES (@ThingsDescription, @ThingsDueDate, @ThingsCategoryId);", conn);

      SqlParameter descriptionParameter = new SqlParameter("@ThingsDescription", this.GetDescription());
      descriptionParameter.ParameterName = "@ThingsDescription";
      descriptionParameter.Value = this.GetDescription();

      SqlParameter dueDateParameter = new SqlParameter("@ThingsDueDate", this.GetDueDate());
      dueDateParameter.ParameterName = "@ThingsDueDate";
      dueDateParameter.Value = this.GetDueDate();

      SqlParameter categoryIdParameter = new SqlParameter();
      categoryIdParameter.ParameterName = "@ThingsCategoryId";
      categoryIdParameter.Value = this.GetCategoryId();

      cmd.Parameters.Add(descriptionParameter);
      cmd.Parameters.Add(categoryIdParameter);
      cmd.Parameters.Add(dueDateParameter);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }
    public static List<Collector> GetAll()
    {
      List<Collector> allCollectors = new List<Collector>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM Things;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int ThingId = rdr.GetInt32(0);
        string ThingDescription = rdr.GetString(1);
        DateTime ThingDateTime = rdr.GetDateTime(2);
        int ThingCategoryId = rdr.GetInt32(3);
        Collector newCollector = new Collector(ThingDescription, ThingCategoryId, ThingDateTime, ThingId);
        allCollectors.Add(newCollector);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allCollectors;
    }
    public static Collector Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM Things WHERE id = @ThingsId;", conn);
      SqlParameter thingsIdParameter = new SqlParameter();
      thingsIdParameter.ParameterName = "@ThingsId";
      thingsIdParameter.Value = id.ToString();
      cmd.Parameters.Add(thingsIdParameter);
      rdr = cmd.ExecuteReader();

      int foundCollectorId = 0;
      string foundCollectorDescription = null;
      DateTime foundCollectorDateTime = new DateTime(2099, 12, 31);
      int foundCategoryId = 0;
      while(rdr.Read())
      {
        foundCollectorId = rdr.GetInt32(0);
        foundCollectorDescription = rdr.GetString(1);
        foundCollectorDateTime = rdr.GetDateTime(2);
        foundCategoryId = rdr.GetInt32(3);
      }
      Collector foundCollector = new Collector(foundCollectorDescription, foundCategoryId, foundCollectorDateTime, foundCollectorId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return foundCollector;
    }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM things;", conn);
      cmd.ExecuteNonQuery();
    }
  }
}
