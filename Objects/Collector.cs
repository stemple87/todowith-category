using System.Collections.Generic;
using System;
using System.Data.SqlClient;

namespace CollectorNS.Objects
{
  public class Collector
  {
    private int _id;
    private string _description;

    public Collector(string Description, int Id = 0)
    {
      _id = Id;
      _description = Description;
      if(_id == 0)
        Save();
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
        return (idEquality && descriptionEquality);
      }
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO things (description) OUTPUT INSERTED.id VALUES (@ThingsDescription);", conn);

      SqlParameter descriptionParameter = new SqlParameter("@ThingsDescription", this.GetDescription());
      Console.WriteLine(descriptionParameter.Value);
      // descriptionParameter.ParameterName = "@ThingsDescription";
      // descriptionParameter.Value = this.GetDescription();
      cmd.Parameters.Add(descriptionParameter);
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
        Collector newCollector = new Collector(ThingDescription, ThingId);
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
      while(rdr.Read())
      {
        foundCollectorId = rdr.GetInt32(0);
        foundCollectorDescription = rdr.GetString(1);
      }
      Collector foundCollector = new Collector(foundCollectorDescription, foundCollectorId);

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
