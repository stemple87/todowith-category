using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace CollectorNS
{
  public class CollectorTest : IDisposable
  {
    public CollectorTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=things_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = Collector.GetAll().Count;

      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfDescriptionsAreTheSame()
    {
      //Arrange, Act
      DateTime dummyDate = new DateTime(2016, 1, 1);
      Collector firstCollector = new Collector("Mow the lawn", 1, dummyDate);
      Collector secondCollector = new Collector("Mow the lawn", 1, dummyDate);

      //Assert
      Assert.Equal(firstCollector, secondCollector);
    }

    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      //Arrange
      DateTime dummyDate = new DateTime(2016, 1, 1);
      Collector testCollector = new Collector("Mow the lawn", 1, dummyDate);

      //Act
      testCollector.Save();
      Collector savedCollector = Collector.GetAll()[0];

      int result = savedCollector.GetId();
      int testId = testCollector.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsCollectorInDatabase()
    {
      //Arrange
      DateTime dummyDate = new DateTime(2016, 1, 1);
      Collector testCollector = new Collector("Mow the lawn", 1, dummyDate);
      testCollector.Save();

      //Act
      Collector foundCollector = Collector.Find(testCollector.GetId());

      //Assert
      Assert.Equal(testCollector, foundCollector);
    }

    public void Dispose()
    {
      Collector.DeleteAll();
    }
  }
}
