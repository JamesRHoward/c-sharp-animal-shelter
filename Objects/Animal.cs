using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AnimalShelter
{
  public class Animal
  {
    private int _id;
    private string _name;
    private string _breed;
    private string _gender;
    private int _age;

    public Animal(string name, string breed, string gender, int age, int id = 0)
    {
      _id = id;
      _name = name;
      _gender = gender;
      _breed = breed;
      _age = age;
    }

    public int GetId()
    {
      return _id;
    }
    public void SetId(int newId)
    {
      _id = newId;
    }

    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }

    public string GetBreed()
    {
      return _breed;
    }
    public void SetBreed(string newBreed)
    {
      _breed = newBreed;
    }

    public string GetGender()
    {
      return _gender;
    }
    public void SetGender(string newGender)
    {
      _gender = newGender;
    }

    public int GetAge()
    {
      return _age;
    }
    public void SetAge(int newAge)
    {
      _age = newAge;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO animals (name, breed, gender, age) OUTPUT INSERTED.id VALUES (@animalName, @animalBreed, @animalGender, @animalAge);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@animalName";
      nameParameter.Value = this.GetName();

      SqlParameter breedParameter = new SqlParameter();
      breedParameter.ParameterName = "@animalBreed";
      breedParameter.Value = this.GetBreed();

      SqlParameter genderParameter = new SqlParameter();
      genderParameter.ParameterName = "@animalGender";
      genderParameter.Value = this.GetGender();

      SqlParameter ageParameter = new SqlParameter();
      ageParameter.ParameterName = "@animalAge";
      ageParameter.Value = this.GetAge();

      cmd.Parameters.Add(nameParameter);
      cmd.Parameters.Add(breedParameter);
      cmd.Parameters.Add(genderParameter);
      cmd.Parameters.Add(ageParameter);

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

    public static List<Animal> GetAll()
    {
      List<Animal> allAnimals = new List<Animal>{};
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM animals;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int animalId = rdr.GetInt32(0);
        string animalName = rdr.GetString(1);
        string animalBreed = rdr.GetString(2);
        string animalGender = rdr.GetString(3);
        int animalAge = rdr.GetInt32(4);
        Animal newAnimal = new Animal(animalName, animalBreed, animalGender, animalAge, animalId);
        allAnimals.Add(newAnimal);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allAnimals;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM animals;", conn);
      cmd.ExecuteNonQuery();
    }

  }
}
