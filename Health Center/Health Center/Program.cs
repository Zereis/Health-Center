using Npgsql;
using System.Data;
using System.Globalization;
{
    //var version = cmd.ExecuteScalar().ToString();
    //Console.WriteLine($"PostgreSQL version: {version}");
    var cs = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

    using var con = new NpgsqlConnection(cs);
    con.Open();

    string patientList = "SELECT * FROM patient";
    using var cmd = new NpgsqlCommand(patientList, con);

    using NpgsqlDataReader rdr = cmd.ExecuteReader();

    string[] storeID = new string[100];
    int n = 0;

    bool found = false;
    string medicalID;
    string createNewMedicalID;
    string selectedDoctor = null;

    while (rdr.Read())
    {
        //Console.WriteLine("{0} {1} {2} {3} {4} {5} {6} {7}", rdr.GetInt32(0), rdr.GetString(1),
        //      rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetInt32(5), rdr.GetDate(6), rdr.GetDate(7));


        storeID[n] = rdr.GetInt32(0).ToString();
        n++;
    }



    Console.WriteLine("Welcome to HEALTH CENTER");
    Console.WriteLine("Login as: ");
    Console.WriteLine("");
    Console.WriteLine("1. Patient");
    Console.WriteLine("2. Doctor");
    Console.WriteLine("3: Administrator");
    Console.WriteLine("0. Exit");
    int choice = Convert.ToInt32(Console.ReadLine());

    switch (choice)
    {
        case 1:
            Console.WriteLine("Are you an already registered patient? (y/n)");
            string response = Console.ReadLine();
            if (response == "y")
            {

                Console.WriteLine("Enter 9-digit Medical ID: ");
                medicalID = Console.ReadLine();

                if (medicalID.Length == 9)
                {
                    for (int i = 0; i < storeID.Length; i++)
                    {
                        if (medicalID == storeID[i])
                        {
                            found = true;
                        }

                    }
                }


                while (found == false)
                {
                    Console.WriteLine("Login failed! Please Re-Enter 9-digit Medical ID: ");
                    medicalID = Console.ReadLine();

                    for (int i = 0; i < storeID.Length; i++)
                    {
                        if (medicalID == storeID[i])
                        {
                            found = true;
                        }

                    }

                }


                if (found == true)
                {
                    Console.WriteLine("-- You have successfully logged in --");
                    Console.WriteLine("--- PATIENT MENU ---");
                    Console.WriteLine("1. View my information");
                    Console.WriteLine("2. Update my information");
                    Console.WriteLine("3: Show doctors");
                    Console.WriteLine("4: Search for specialization");
                    Console.WriteLine("5: Select doctor");
                    Console.WriteLine("6: Show available days for selected doctor");
                    Console.WriteLine("7: Book appointment with selected doctor");
                    Console.WriteLine("8: View the diagnos and prescription");
                    Console.WriteLine("0. Exit");
                    int patientChoice = Convert.ToInt32(Console.ReadLine());

                    while (patientChoice != 0)
                    {
                        if(patientChoice == 1)
                        {
                            // show information
                            var css = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                            using var conn = new NpgsqlConnection(css);
                            conn.Open();


    
                            // call function getpatientbyid
                            //string patientinfo = "SELECT * FROM getpatientbyid (@test)";
                            using var cmdd = new NpgsqlCommand("SELECT * FROM getpatientbyid (@medicalid)", conn);
                            int medicalnumber = int.Parse(medicalID);

                            cmdd.Parameters.AddWithValue("medicalid", medicalnumber);
                            using NpgsqlDataReader rdrr = cmdd.ExecuteReader();

                            Console.WriteLine("MedicalID  FirstName  LastName  Gender  Adress     Phone     Birthdate  Registrationdate");
                            while (rdrr.Read())
                            {
                                Console.WriteLine("{0}  {1}      {2}     {3}    {4} {5} {6} {7}", rdrr.GetInt32(0), rdrr.GetString(1),
                                      rdrr.GetString(2), rdrr.GetString(3), rdrr.GetString(4), rdrr.GetInt32(5), rdrr.GetDate(6), rdrr.GetDate(7));

                            }
                            Console.WriteLine("--- PATIENT MENU ---");
                            Console.WriteLine("1. View my information");
                            Console.WriteLine("2. Update my information");
                            Console.WriteLine("3: Show doctors");
                            Console.WriteLine("4: Search for specialization");
                            Console.WriteLine("5: Select doctor");
                            Console.WriteLine("6: Show available days for selected doctor");
                            Console.WriteLine("7: Book appointment with selected doctor");
                            Console.WriteLine("8: View the diagnos and prescription");
                            Console.WriteLine("0. Exit");
                            patientChoice = Convert.ToInt32(Console.ReadLine());
                        }
                        else if (patientChoice == 2)
                        {
                            // update info
                            Console.WriteLine("--Update Patient Info--");
                            Console.WriteLine("Please enter a new first name");
                            string updateFirstName = Console.ReadLine();
                            Console.WriteLine("Please enter a new last name");
                            string updateLastName = Console.ReadLine();
                            Console.WriteLine("Please enter your new gender: (Male/Female)");
                            string updateGender = Console.ReadLine();
                            Console.WriteLine("Please enter your new address:");
                            string updateAdress = Console.ReadLine();
                            Console.WriteLine("Please enter your new phone number:");
                            string updatePhoneNumber = Console.ReadLine();
                            Console.WriteLine("Please enter the new year you were born:");
                            string updateYear = Console.ReadLine();
                            Console.WriteLine("Please enter the new month you were born:");
                            string updateMonth = Console.ReadLine();
                            Console.WriteLine("Please enter the new day you were born: ");
                            string updateDay = Console.ReadLine();

                            DateOnly newbirthdate = new DateOnly(Int32.Parse(updateYear), Int32.Parse(updateMonth), Int32.Parse(updateDay));
                            var css = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                            using var conn = new NpgsqlConnection(css);
                            conn.Open();



                            // call storedprocedures to update info
                            using var cmdd = new NpgsqlCommand("call updatepatientinfo(@medicalid, @newfirstname, @newlastname, @newgender,@newadress, @newphone,@newbirthdate)", conn);
                            int medicalid = int.Parse(medicalID);

                            cmdd.Parameters.AddWithValue("medicalid", medicalid);
                            cmdd.Parameters.AddWithValue("newfirstname", updateFirstName);
                            cmdd.Parameters.AddWithValue("newlastname", updateLastName);
                            cmdd.Parameters.AddWithValue("newgender", updateGender);
                            cmdd.Parameters.AddWithValue("newadress", updateAdress);
                            cmdd.Parameters.AddWithValue("newphone", int.Parse(updatePhoneNumber));
                            cmdd.Parameters.AddWithValue("newbirthdate", newbirthdate);


                            using NpgsqlDataReader rdrr = cmdd.ExecuteReader();
                            Console.WriteLine("You have updated your info");
                            Console.WriteLine("");
                            Console.WriteLine("--- PATIENT MENU ---");
                            Console.WriteLine("1. View my information");
                            Console.WriteLine("2. Update my information");
                            Console.WriteLine("3: Show doctors");
                            Console.WriteLine("4: Search for specialization");
                            Console.WriteLine("5: Select doctor");
                            Console.WriteLine("6: Show available days for selected doctor");
                            Console.WriteLine("7: Book appointment with selected doctor");
                            Console.WriteLine("8: View the diagnos and prescription");
                            Console.WriteLine("0. Exit");
                            patientChoice = Convert.ToInt32(Console.ReadLine());

                        }
                        else if (patientChoice == 3)
                        {
                            // show doctors 
                            // Use select * doctors
                            var test = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                            using var connect = new NpgsqlConnection(test);
                            connect.Open();

                            string showDoctor = "SELECT * FROM doctor";
                            using var command = new NpgsqlCommand(showDoctor, connect);

                            using NpgsqlDataReader reader = command.ExecuteReader();


                            Console.WriteLine("EmployeeID    Name    Specialization    Cost   Number");
                            while (reader.Read())
                            {
                                Console.WriteLine("{0} {1} {2} {3} {4}", reader.GetInt32(0), reader.GetString(1),
                                     reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4));
                            }

                            Console.WriteLine("--- PATIENT MENU ---");
                            Console.WriteLine("1. View my information");
                            Console.WriteLine("2. Update my information");
                            Console.WriteLine("3: Show doctors");
                            Console.WriteLine("4: Search for specialization");
                            Console.WriteLine("5: Select doctor");
                            Console.WriteLine("6: Show available days for selected doctor");
                            Console.WriteLine("7: Book appointment with selected doctor");
                            Console.WriteLine("8: View the diagnos and prescription");
                            Console.WriteLine("0. Exit");
                            patientChoice = Convert.ToInt32(Console.ReadLine());
                        }
                        else if(patientChoice == 4)
                        {
                            // search for specialization
                            // use select * doctors where search specialization

                            var test = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                            using var connect = new NpgsqlConnection(test);
                            connect.Open();

                            string showDoctor = "select * from doctor where doctor.specialization = @spec";
                            Console.WriteLine("What doctor are you looking for?");
                            string searchDoctor = Console.ReadLine();
    
                            using var command = new NpgsqlCommand(showDoctor, connect);
                            command.Parameters.AddWithValue("spec", searchDoctor);
                            using NpgsqlDataReader reader = command.ExecuteReader();


                            Console.WriteLine("EmployeeID    Name    Specialization    Cost   Number");
                            while (reader.Read())
                            {
                                Console.WriteLine("{0} {1} {2} {3} {4}", reader.GetInt32(0), reader.GetString(1),
                                     reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4));
                            }
                            Console.WriteLine();
                            Console.WriteLine("--- PATIENT MENU ---");
                            Console.WriteLine("1. View my information");
                            Console.WriteLine("2. Update my information");
                            Console.WriteLine("3: Show doctors");
                            Console.WriteLine("4: Search for specialization");
                            Console.WriteLine("5: Select doctor");
                            Console.WriteLine("6: Show available days for selected doctor");
                            Console.WriteLine("7: Book appointment with selected doctor");
                            Console.WriteLine("8: View the diagnos and prescription");
                            Console.WriteLine("0. Exit");
                            patientChoice = Convert.ToInt32(Console.ReadLine());
                        }
                        else if (patientChoice == 5)
                        {
                            // select doctor
                            Console.WriteLine("Selecty a doctor by entering their employee id");
 

                            var css = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                            using var conn = new NpgsqlConnection(css);
                            conn.Open();



                            // call function getpatientbyid
                            //string patientinfo = "SELECT * FROM getpatientbyid (@test)";
                            using var cmdd = new NpgsqlCommand("SELECT * FROM getdoctor(@doctorid)", conn);
                            int selectDoctor = int.Parse(Console.ReadLine());

                            cmdd.Parameters.AddWithValue("doctorid", selectDoctor) ;
                            using NpgsqlDataReader rdrr = cmdd.ExecuteReader();


                            while (rdrr.Read())
                            {
                                Console.WriteLine("You have selected:" + " {0} {1}" + " as doctor", rdrr.GetInt32(0), rdrr.GetString(1));
                                selectedDoctor = rdrr.GetInt32(0).ToString();



                            }
       
                            Console.WriteLine();
                            Console.WriteLine("--- PATIENT MENU ---");
                            Console.WriteLine("1. View my information");
                            Console.WriteLine("2. Update my information");
                            Console.WriteLine("3: Show doctors");
                            Console.WriteLine("4: Search for specialization");
                            Console.WriteLine("5: Select doctor");
                            Console.WriteLine("6: Show available days for selected doctor");
                            Console.WriteLine("7: Book appointment with selected doctor");
                            Console.WriteLine("8: View the diagnos and prescription");
                            Console.WriteLine("0. Exit");
                            patientChoice = Convert.ToInt32(Console.ReadLine());
                        }
                        else if (patientChoice == 6)
                        {
                            int dentistid = 0;
                            // show available days for selected doctor
                            if(int.Parse(selectedDoctor) == 12345)
                            {
                                var database = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                                using var connect = new NpgsqlConnection(database);
                                connect.Open();

                                string dentistSchedule = "SELECT * FROM scheduledentist where isbooked = false";
                                using var com = new NpgsqlCommand(dentistSchedule, connect);

                                using NpgsqlDataReader reads = com.ExecuteReader();

                            
                                while (reads.Read())
                                {
                                    Console.WriteLine("{0} {1} {2}  ", reads.GetString(0), reads.GetDate(1),
                                          reads.GetTimeSpan(2));
                                }

                                Console.WriteLine();
                                Console.WriteLine("--- PATIENT MENU ---");
                                Console.WriteLine("1. View my information");
                                Console.WriteLine("2. Update my information");
                                Console.WriteLine("3: Show doctors");
                                Console.WriteLine("4: Search for specialization");
                                Console.WriteLine("5: Select doctor");
                                Console.WriteLine("6: Show available days for selected doctor");
                                Console.WriteLine("7: Book appointment with selected doctor");
                                Console.WriteLine("8: View the diagnos and prescription");
                                Console.WriteLine("0. Exit");

                                patientChoice = Convert.ToInt32(Console.ReadLine());
                            }
                            else if (int.Parse(selectedDoctor) == 12347)
                            {
                                var database = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                                using var connect = new NpgsqlConnection(database);
                                connect.Open();

                                string dentistSchedule = "SELECT * FROM schedulecardiologist where isbooked = false";
                                using var com = new NpgsqlCommand(dentistSchedule, connect);

                                using NpgsqlDataReader reads = com.ExecuteReader();


                                while (reads.Read())
                                {
                                    Console.WriteLine("{0} {1} {2}  ", reads.GetString(0), reads.GetDate(1),
                                    reads.GetTimeSpan(2));
                                }
                            }
                            else if (int.Parse(selectedDoctor) == 12349)
                            {
                                var database = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                                using var connect = new NpgsqlConnection(database);
                                connect.Open();

                                string dentistSchedule = "SELECT * FROM schedulephychiastrists where isbooked = false";
                                using var com = new NpgsqlCommand(dentistSchedule, connect);

                                using NpgsqlDataReader reads = com.ExecuteReader();


                                while (reads.Read())
                                {
                                    Console.WriteLine("{0} {1} {2}  ", reads.GetString(0), reads.GetDate(1),
                                    reads.GetTimeSpan(2));
                                }
                            }
                            while (selectedDoctor == null)
                            {
                                Console.WriteLine("Please select a doctor first");
                                Console.WriteLine();
                                Console.WriteLine("--- PATIENT MENU ---");
                                Console.WriteLine("1. View my information");
                                Console.WriteLine("2. Update my information");
                                Console.WriteLine("3: Show doctors");
                                Console.WriteLine("4: Search for specialization");
                                Console.WriteLine("5: Select doctor");
                                Console.WriteLine("6: Show available days for selected doctor");
                                Console.WriteLine("7: Book appointment with selected doctor");
                                Console.WriteLine("8: View the diagnos and prescription");
                                Console.WriteLine("0. Exit");
                                patientChoice = Convert.ToInt32(Console.ReadLine());
                            }
                        }
                        else if (patientChoice == 7)
                        {
                            // Book appointment with selected doctor
                            Console.WriteLine("Which date? (YYYYMMDD)");
                            Console.WriteLine("YYYY:");
                            string bookYear = Console.ReadLine();
                            Console.WriteLine("MM:");
                            string bookMonth = Console.ReadLine();
                            Console.WriteLine("DD");
                            string bookDay = Console.ReadLine();

                            DateOnly appointmantdate = new DateOnly(Int32.Parse(bookYear), Int32.Parse(bookMonth), Int32.Parse(bookDay));
                            Console.WriteLine("Which time? (HHMM)");
                            Console.WriteLine("HH");
                            string hour = Console.ReadLine();
                            Console.WriteLine("MM");
                            string minute = Console.ReadLine();
                            TimeOnly timeOnly = new TimeOnly(Int32.Parse(hour), Int32.Parse(minute));

                            var css = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                            using var conn = new NpgsqlConnection(css);
                            conn.Open();
                            bool confirmed = true;
                            // call storedprocedures to update info
                            using var cmdd = new NpgsqlCommand("call bookappointment(@bookdate, @booktime, @confirmed)", conn);

                            cmdd.Parameters.AddWithValue("bookdate", appointmantdate);
                            cmdd.Parameters.AddWithValue("booktime", timeOnly);
                            cmdd.Parameters.AddWithValue("confirmed", confirmed);

                            using NpgsqlDataReader rdrr = cmdd.ExecuteReader();

                            Console.WriteLine("You have successfully booked an appointment");
                            Console.WriteLine("");
                            Console.WriteLine("--- PATIENT MENU ---");
                            Console.WriteLine("1. View my information");
                            Console.WriteLine("2. Update my information");
                            Console.WriteLine("3: Show doctors");
                            Console.WriteLine("4: Search for specialization");
                            Console.WriteLine("5: Select doctor");
                            Console.WriteLine("6: Show available days for selected doctor");
                            Console.WriteLine("7: Book appointment with selected doctor");
                            Console.WriteLine("8: View the diagnos and prescription");
                            Console.WriteLine("0. Exit");
                            patientChoice = Convert.ToInt32(Console.ReadLine());
                        }
                        else if (patientChoice == 8)
                        {
                            // View doctors diagnos
                            var css = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                            using var conn = new NpgsqlConnection(css);
                            conn.Open();



                            // call function getpatientbyid
                            //string patientinfo = "SELECT * FROM getpatientbyid (@test)";
                            using var cmdd = new NpgsqlCommand("select * from getpatientdiagnosandprescription(@patientid,@doctorid);", conn);
                            int patientid = int.Parse(medicalID);
                            int doctorid = int.Parse(selectedDoctor);
                            cmdd.Parameters.AddWithValue("patientid", patientid);
                            cmdd.Parameters.AddWithValue("doctorid", doctorid);
                            using NpgsqlDataReader rdrr = cmdd.ExecuteReader();
                            Console.WriteLine("Here is the patient ID and the doctors ID and the appointment with diagnos and prescription");

                            while (rdrr.Read())
                            {
                                Console.WriteLine("{0} {1} {2} {3} {4}   ", rdrr.GetInt32(0), rdrr.GetInt32(1),
                                rdrr.GetDate(2), rdrr.GetString(3), rdrr.GetString(4));
                            }

                            Console.WriteLine("--- PATIENT MENU ---");
                            Console.WriteLine("1. View my information");
                            Console.WriteLine("2. Update my information");
                            Console.WriteLine("3: Show doctors");
                            Console.WriteLine("4: Search for specialization");
                            Console.WriteLine("5: Select doctor");
                            Console.WriteLine("6: Show available days for selected doctor");
                            Console.WriteLine("7: Book appointment with selected doctor");
                            Console.WriteLine("8: View the diagnos and prescription");
                            Console.WriteLine("0. Exit");
                            patientChoice = Convert.ToInt32(Console.ReadLine());
                        }


                    }
                    Console.WriteLine("Closing application");
                }



            }
            else
            {
                Console.WriteLine("--- PATIENT REGISTRATION ---");
                Console.WriteLine("Please enter your medical number:");
                string medicalNumber = Console.ReadLine();
                createNewMedicalID = medicalNumber;
                while(createNewMedicalID.Length != 9)
                {
                    Console.WriteLine("Please enter 9 digits medical numbers:");
                    createNewMedicalID = Console.ReadLine();
                }
                Console.WriteLine("Please enter your first name");
                string firstName = Console.ReadLine();
                Console.WriteLine("Please enter your last name");
                string lastName = Console.ReadLine();
                Console.WriteLine("Please enter your gender: (Male/Female)");
                string gender = Console.ReadLine();
                Console.WriteLine("Please enter your address:");
                string adress = Console.ReadLine();
                Console.WriteLine("Please enter your phone number:");
                string phoneNumber = Console.ReadLine();
                Console.WriteLine("Please enter the year you were born:");
                string year = Console.ReadLine();
                Console.WriteLine("Please enter the month you were born:");
                string month = Console.ReadLine();
                Console.WriteLine("Please enter the day you were born: ");
                string day = Console.ReadLine();

                DateTime today = DateTime.Today;

                DateOnly birthdate = new DateOnly(Int32.Parse(year), Int32.Parse(month), Int32.Parse(day));
                DateOnly regdate = new DateOnly(today.Year, today.Month, today.Day);
                //cmd.CommandText = "INSERT INTO patient(medicalnumber, firstname, lastname, gender, address, phone, birthdate, registration) VALUES('188113274, bar, bla, Male, testgatan11, 0700707007, 1999-01-01, 1999-01-01')";
                //cmd.ExecuteNonQuery();
                var cse = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";
                using var cons = new NpgsqlConnection(cse);
                cons.Open();

                var sql = "INSERT INTO patient(medicalnumber, firstname, lastname, gender, address, phone, birthdate, registration) VALUES(@medicalnumber, @firstname, @lastname, @gender, @address, @phone, @birthdate, @registrationdate) ";
                using var cmde = new NpgsqlCommand(sql, cons);

                cmde.Parameters.AddWithValue("medicalnumber", int.Parse(medicalNumber));
                cmde.Parameters.AddWithValue("firstname", firstName);
                cmde.Parameters.AddWithValue("lastname", lastName);
                cmde.Parameters.AddWithValue("gender", gender);
                cmde.Parameters.AddWithValue("address", adress);
                cmde.Parameters.AddWithValue("phone", int.Parse(phoneNumber));
                cmde.Parameters.AddWithValue("birthdate", birthdate);
                cmde.Parameters.AddWithValue("registrationdate", regdate);
                cmde.Prepare();

                cmde.ExecuteNonQuery();

                Console.WriteLine("-- You have successfully created an account --");
                Console.WriteLine("--- PATIENT MENU ---");
                Console.WriteLine("1. View my information");

                Console.WriteLine("2. Update my information");
                Console.WriteLine("3: Show doctors");
                Console.WriteLine("4: Search for specialization");
                Console.WriteLine("5: Select doctor");
                Console.WriteLine("6: Show available days for selected doctor");
                Console.WriteLine("7: Book appointment with selected doctor");
                Console.WriteLine("8: View the diagnos and prescription");
                Console.WriteLine("0. Exit");

                int patientChoice = Convert.ToInt32(Console.ReadLine());

                while (patientChoice != 0)
                {
                    if (patientChoice == 1)
                    {
                        // show information
                        var css = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                        using var conn = new NpgsqlConnection(css);
                        conn.Open();



                        // call function getpatientbyid
                        //string patientinfo = "SELECT * FROM getpatientbyid (@test)";
                        using var cmdd = new NpgsqlCommand("SELECT * FROM getpatientbyid (@medicalid)", conn);
                        int medicalnumber = int.Parse(createNewMedicalID);

                        cmdd.Parameters.AddWithValue("medicalid", medicalnumber);
                        using NpgsqlDataReader rdrr = cmdd.ExecuteReader();

                        Console.WriteLine("MedicalID  FirstName  LastName  Gender  Adress     Phone     Birthdate  Registrationdate");
                        while (rdrr.Read())
                        {
                            Console.WriteLine("{0}  {1}      {2}     {3}    {4} {5} {6} {7}", rdrr.GetInt32(0), rdrr.GetString(1),
                                  rdrr.GetString(2), rdrr.GetString(3), rdrr.GetString(4), rdrr.GetInt32(5), rdrr.GetDate(6), rdrr.GetDate(7));

                        }
                        Console.WriteLine("--- PATIENT MENU ---");
                        Console.WriteLine("1. View my information");
                        Console.WriteLine("2. Update my information");
                        Console.WriteLine("3: Show doctors");
                        Console.WriteLine("4: Search for specialization");
                        Console.WriteLine("5: Select doctor");
                        Console.WriteLine("6: Show available days for selected doctor");
                        Console.WriteLine("7: Book appointment with selected doctor");
                        Console.WriteLine("8: View the diagnos and prescription");
                        Console.WriteLine("0. Exit");
                        patientChoice = Convert.ToInt32(Console.ReadLine());
                    }
                    else if (patientChoice == 2)
                    {
                        // update info
                        Console.WriteLine("--Update Patient Info--");
                        Console.WriteLine("Please enter a new first name");
                        string updateFirstName = Console.ReadLine();
                        Console.WriteLine("Please enter a new last name");
                        string updateLastName = Console.ReadLine();
                        Console.WriteLine("Please enter your new gender: (Male/Female)");
                        string updateGender = Console.ReadLine();
                        Console.WriteLine("Please enter your new address:");
                        string updateAdress = Console.ReadLine();
                        Console.WriteLine("Please enter your new phone number:");
                        string updatePhoneNumber = Console.ReadLine();
                        Console.WriteLine("Please enter the new year you were born:");
                        string updateYear = Console.ReadLine();
                        Console.WriteLine("Please enter the new month you were born:");
                        string updateMonth = Console.ReadLine();
                        Console.WriteLine("Please enter the new day you were born: ");
                        string updateDay = Console.ReadLine();

                        DateOnly newbirthdate = new DateOnly(Int32.Parse(updateYear), Int32.Parse(updateMonth), Int32.Parse(updateDay));
                        var css = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                        using var conn = new NpgsqlConnection(css);
                        conn.Open();



                        // call storedprocedures to update info
                        using var cmdd = new NpgsqlCommand("call updatepatientinfo(@medicalid, @newfirstname, @newlastname, @newgender,@newadress, @newphone,@newbirthdate)", conn);
                        int medicalid = int.Parse(createNewMedicalID);

                        cmdd.Parameters.AddWithValue("medicalid", medicalid);
                        cmdd.Parameters.AddWithValue("newfirstname", updateFirstName);
                        cmdd.Parameters.AddWithValue("newlastname", updateLastName);
                        cmdd.Parameters.AddWithValue("newgender", updateGender);
                        cmdd.Parameters.AddWithValue("newadress", updateAdress);
                        cmdd.Parameters.AddWithValue("newphone", int.Parse(updatePhoneNumber));
                        cmdd.Parameters.AddWithValue("newbirthdate", newbirthdate);


                        using NpgsqlDataReader rdrr = cmdd.ExecuteReader();
                        Console.WriteLine("You have updated your info");
                        Console.WriteLine("");
                        Console.WriteLine("--- PATIENT MENU ---");
                        Console.WriteLine("1. View my information");
                        Console.WriteLine("2. Update my information");
                        Console.WriteLine("3: Show doctors");
                        Console.WriteLine("4: Search for specialization");
                        Console.WriteLine("5: Select doctor");
                        Console.WriteLine("6: Show available days for selected doctor");
                        Console.WriteLine("7: Book appointment with selected doctor");
                        Console.WriteLine("8: View the diagnos and prescription");
                        Console.WriteLine("0. Exit");
                        patientChoice = Convert.ToInt32(Console.ReadLine());

                    }
                    else if (patientChoice == 3)
                    {
                        // show doctors 
                        // Use select * doctors
                        var test = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                        using var connect = new NpgsqlConnection(test);
                        connect.Open();

                        string showDoctor = "SELECT * FROM doctor";
                        using var command = new NpgsqlCommand(showDoctor, connect);

                        using NpgsqlDataReader reader = command.ExecuteReader();


                        Console.WriteLine("EmployeeID    Name    Specialization    Cost   Number");
                        while (reader.Read())
                        {
                            Console.WriteLine("{0} {1} {2} {3} {4}", reader.GetInt32(0), reader.GetString(1),
                                 reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4));
                        }

                        Console.WriteLine("--- PATIENT MENU ---");
                        Console.WriteLine("1. View my information");
                        Console.WriteLine("2. Update my information");
                        Console.WriteLine("3: Show doctors");
                        Console.WriteLine("4: Search for specialization");
                        Console.WriteLine("5: Select doctor");
                        Console.WriteLine("6: Show available days for selected doctor");
                        Console.WriteLine("7: Book appointment with selected doctor");
                        Console.WriteLine("8: View the diagnos and prescription");
                        Console.WriteLine("0. Exit");
                        patientChoice = Convert.ToInt32(Console.ReadLine());
                    }
                    else if (patientChoice == 4)
                    {
                        // search for specialization
                        // use select * doctors where search specialization

                        var test = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                        using var connect = new NpgsqlConnection(test);
                        connect.Open();

                        string showDoctor = "select * from doctor where doctor.specialization = @spec";
                        Console.WriteLine("What doctor are you looking for?");
                        string searchDoctor = Console.ReadLine();

                        using var command = new NpgsqlCommand(showDoctor, connect);
                        command.Parameters.AddWithValue("spec", searchDoctor);
                        using NpgsqlDataReader reader = command.ExecuteReader();


                        Console.WriteLine("EmployeeID    Name    Specialization    Cost   Number");
                        while (reader.Read())
                        {
                            Console.WriteLine("{0} {1} {2} {3} {4}", reader.GetInt32(0), reader.GetString(1),
                                 reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4));
                        }
                        Console.WriteLine();
                        Console.WriteLine("--- PATIENT MENU ---");
                        Console.WriteLine("1. View my information");
                        Console.WriteLine("2. Update my information");
                        Console.WriteLine("3: Show doctors");
                        Console.WriteLine("4: Search for specialization");
                        Console.WriteLine("5: Select doctor");
                        Console.WriteLine("6: Show available days for selected doctor");
                        Console.WriteLine("7: Book appointment with selected doctor");
                        Console.WriteLine("8: View the diagnos and prescription");
                        Console.WriteLine("0. Exit");
                        patientChoice = Convert.ToInt32(Console.ReadLine());
                    }
                    else if (patientChoice == 5)
                    {
                        // select doctor
                        Console.WriteLine("Selecty a doctor by entering their employee id");


                        var css = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                        using var conn = new NpgsqlConnection(css);
                        conn.Open();



                        // call function getpatientbyid
                        //string patientinfo = "SELECT * FROM getpatientbyid (@test)";
                        using var cmdd = new NpgsqlCommand("SELECT * FROM getdoctor(@doctorid)", conn);
                        int selectDoctor = int.Parse(Console.ReadLine());

                        cmdd.Parameters.AddWithValue("doctorid", selectDoctor);
                        using NpgsqlDataReader rdrr = cmdd.ExecuteReader();


                        while (rdrr.Read())
                        {
                            Console.WriteLine("You have selected:" + " {0} {1}" + " as doctor", rdrr.GetInt32(0), rdrr.GetString(1));
                            selectedDoctor = rdrr.GetInt32(0).ToString();



                        }

                        Console.WriteLine();
                        Console.WriteLine("--- PATIENT MENU ---");
                        Console.WriteLine("1. View my information");
                        Console.WriteLine("2. Update my information");
                        Console.WriteLine("3: Show doctors");
                        Console.WriteLine("4: Search for specialization");
                        Console.WriteLine("5: Select doctor");
                        Console.WriteLine("6: Show available days for selected doctor");
                        Console.WriteLine("7: Book appointment with selected doctor");
                        Console.WriteLine("8: View the diagnos and prescription");
                        Console.WriteLine("0. Exit");
                        patientChoice = Convert.ToInt32(Console.ReadLine());
                    }
                    else if (patientChoice == 6)
                    {
                        int dentistid = 0;
                        // show available days for selected doctor
                        if (int.Parse(selectedDoctor) == 12345)
                        {
                            var database = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                            using var connect = new NpgsqlConnection(database);
                            connect.Open();

                            string dentistSchedule = "SELECT * FROM scheduledentist where isbooked = false";
                            using var com = new NpgsqlCommand(dentistSchedule, connect);

                            using NpgsqlDataReader reads = com.ExecuteReader();


                            while (reads.Read())
                            {
                                Console.WriteLine("{0} {1} {2}  ", reads.GetString(0), reads.GetDate(1),
                                      reads.GetTimeSpan(2));
                            }

                            Console.WriteLine();
                            Console.WriteLine("--- PATIENT MENU ---");
                            Console.WriteLine("1. View my information");
                            Console.WriteLine("2. Update my information");
                            Console.WriteLine("3: Show doctors");
                            Console.WriteLine("4: Search for specialization");
                            Console.WriteLine("5: Select doctor");
                            Console.WriteLine("6: Show available days for selected doctor");
                            Console.WriteLine("7: Book appointment with selected doctor");
                            Console.WriteLine("8: View the diagnos and prescription");
                            Console.WriteLine("0. Exit");

                            patientChoice = Convert.ToInt32(Console.ReadLine());
                        }
                        else if (int.Parse(selectedDoctor) == 12347)
                        {
                            var database = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                            using var connect = new NpgsqlConnection(database);
                            connect.Open();

                            string dentistSchedule = "SELECT * FROM schedulecardiologist where isbooked = false";
                            using var com = new NpgsqlCommand(dentistSchedule, connect);

                            using NpgsqlDataReader reads = com.ExecuteReader();


                            while (reads.Read())
                            {
                                Console.WriteLine("{0} {1} {2}  ", reads.GetString(0), reads.GetDate(1),
                                reads.GetTimeSpan(2));
                            }
                        }
                        else if (int.Parse(selectedDoctor) == 12349)
                        {
                            var database = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                            using var connect = new NpgsqlConnection(database);
                            connect.Open();

                            string dentistSchedule = "SELECT * FROM schedulephychiastrists where isbooked = false";
                            using var com = new NpgsqlCommand(dentistSchedule, connect);

                            using NpgsqlDataReader reads = com.ExecuteReader();


                            while (reads.Read())
                            {
                                Console.WriteLine("{0} {1} {2}  ", reads.GetString(0), reads.GetDate(1),
                                reads.GetTimeSpan(2));
                            }
                        }
                        while (selectedDoctor == null)
                        {
                            Console.WriteLine("Please select a doctor first");
                            Console.WriteLine();
                            Console.WriteLine("--- PATIENT MENU ---");
                            Console.WriteLine("1. View my information");
                            Console.WriteLine("2. Update my information");
                            Console.WriteLine("3: Show doctors");
                            Console.WriteLine("4: Search for specialization");
                            Console.WriteLine("5: Select doctor");
                            Console.WriteLine("6: Show available days for selected doctor");
                            Console.WriteLine("7: Book appointment with selected doctor");
                            Console.WriteLine("8: View the diagnos and prescription");
                            Console.WriteLine("0. Exit");
                            patientChoice = Convert.ToInt32(Console.ReadLine());
                        }
                    }
                    else if (patientChoice == 7)
                    {
                        // Book appointment with selected doctor
                        Console.WriteLine("Which date? (YYYYMMDD)");
                        Console.WriteLine("YYYY:");
                        string bookYear = Console.ReadLine();
                        Console.WriteLine("MM:");
                        string bookMonth = Console.ReadLine();
                        Console.WriteLine("DD");
                        string bookDay = Console.ReadLine();

                        DateOnly appointmantdate = new DateOnly(Int32.Parse(bookYear), Int32.Parse(bookMonth), Int32.Parse(bookDay));
                        Console.WriteLine("Which time? (HHMM)");
                        Console.WriteLine("HH");
                        string hour = Console.ReadLine();
                        Console.WriteLine("MM");
                        string minute = Console.ReadLine();
                        TimeOnly timeOnly = new TimeOnly(Int32.Parse(hour), Int32.Parse(minute));

                        var css = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                        using var conn = new NpgsqlConnection(css);
                        conn.Open();
                        bool confirmed = true;
                        // call storedprocedures to update info
                        using var cmdd = new NpgsqlCommand("call bookappointment(@bookdate, @booktime, @confirmed)", conn);

                        cmdd.Parameters.AddWithValue("bookdate", appointmantdate);
                        cmdd.Parameters.AddWithValue("booktime", timeOnly);
                        cmdd.Parameters.AddWithValue("confirmed", confirmed);

                        using NpgsqlDataReader rdrr = cmdd.ExecuteReader();

                        Console.WriteLine("You have successfully booked an appointment");
                        Console.WriteLine("");
                        Console.WriteLine("--- PATIENT MENU ---");
                        Console.WriteLine("1. View my information");
                        Console.WriteLine("2. Update my information");
                        Console.WriteLine("3: Show doctors");
                        Console.WriteLine("4: Search for specialization");
                        Console.WriteLine("5: Select doctor");
                        Console.WriteLine("6: Show available days for selected doctor");
                        Console.WriteLine("7: Book appointment with selected doctor");
                        Console.WriteLine("8: View the diagnos and prescription");
                        Console.WriteLine("0. Exit");
                        patientChoice = Convert.ToInt32(Console.ReadLine());
                    }
                    else if (patientChoice == 8)
                    {
                        // View doctors diagnos
                        var css = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                        using var conn = new NpgsqlConnection(css);
                        conn.Open();



                        // call function getpatientbyid
                        //string patientinfo = "SELECT * FROM getpatientbyid (@test)";
                        using var cmdd = new NpgsqlCommand("select * from getpatientdiagnosandprescription(@patientid,@doctorid);", conn);
                        int patientid = int.Parse(createNewMedicalID);
                        int doctorid = int.Parse(selectedDoctor);
                        cmdd.Parameters.AddWithValue("patientid", patientid);
                        cmdd.Parameters.AddWithValue("doctorid", doctorid);
                        using NpgsqlDataReader rdrr = cmdd.ExecuteReader();
                        Console.WriteLine("Here is the patient ID and the doctors ID and the appointment with diagnos and prescription");

                        while (rdrr.Read())
                        {
                            Console.WriteLine("{0} {1} {2} {3} {4}   ", rdrr.GetInt32(0), rdrr.GetInt32(1),
                            rdrr.GetDate(2), rdrr.GetString(3), rdrr.GetString(4));
                        }

                        Console.WriteLine("--- PATIENT MENU ---");
                        Console.WriteLine("1. View my information");
                        Console.WriteLine("2. Update my information");
                        Console.WriteLine("3: Show doctors");
                        Console.WriteLine("4: Search for specialization");
                        Console.WriteLine("5: Select doctor");
                        Console.WriteLine("6: Show available days for selected doctor");
                        Console.WriteLine("7: Book appointment with selected doctor");
                        Console.WriteLine("8: View the diagnos and prescription");
                        Console.WriteLine("0. Exit");
                        patientChoice = Convert.ToInt32(Console.ReadLine());
                    }


                }
                Console.WriteLine("Closing application");
            }
            break;
        case 2:

            string[] loginAsDoctor = new string[100];
            bool foundDoctorLogin = false;
            int x = 0;

            if (foundDoctorLogin == false)
            {
                var admin = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                using var connect = new NpgsqlConnection(admin);
                connect.Open();

                string doctorid = "SELECT * FROM doctor";
                using var cmdx = new NpgsqlCommand(doctorid, connect);

                using NpgsqlDataReader rdrr = cmdx.ExecuteReader();

                while (rdrr.Read())
                {
                    //Console.WriteLine("{0} {1} {2} {3} {4} {5} {6} {7}", rdr.GetInt32(0), rdr.GetString(1),
                    //      rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetInt32(5), rdr.GetDate(6), rdr.GetDate(7));


                    loginAsDoctor[x] = rdrr.GetInt32(0).ToString();
                    x++;
                }
            }
            Console.WriteLine("Please type the password to login");
            string doctorlogin = Console.ReadLine();


            for (int i = 0; i < loginAsDoctor.Length; i++)
            {
                if (doctorlogin == loginAsDoctor[i])
                {
                    foundDoctorLogin = true;
                }
            }

            while (foundDoctorLogin == false)
            {
                Console.WriteLine("Login failed! Please Re-Enter 5-digit Medical ID: ");
                doctorlogin = Console.ReadLine();

                for (int i = 0; i < storeID.Length; i++)
                {
                    if (doctorlogin == loginAsDoctor[i])
                    {
                        foundDoctorLogin = true;
                    }

                }

            }


            Console.WriteLine("--- DOCTOR MENU ---");
            Console.WriteLine("");
            Console.WriteLine("1. Availabilities");
            Console.WriteLine("2. Update Schedule");
            Console.WriteLine("3: See Schedule");
            Console.WriteLine("4: List Patient Medical Records");
            Console.WriteLine("0. Exit");
            int doctorchoice = Convert.ToInt32(Console.ReadLine());

            while (doctorchoice != 0)
            {
                if(doctorchoice == 1)
                {
                    var admin = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                    using var connect = new NpgsqlConnection(admin);
                    connect.Open();

                    if(doctorlogin == "12345")
                    {
                        string adminid = "SELECT todaysday, weeklydate, available, isbooked FROM scheduledentist where isbooked = false order by weeklydate, available ";
                        using var cmdx = new NpgsqlCommand(adminid, connect);

                        using NpgsqlDataReader rdrs = cmdx.ExecuteReader();
                        while (rdrs.Read())
                        {
                            Console.WriteLine("{0} {1} {2} ", rdrs.GetString(0), rdrs.GetDate(1),
                                  rdrs.GetTimeSpan(2));

                        }
                    }
                    else if(doctorlogin == "12347")
                    {
                        string adminid = "SELECT * FROM schedulecardiologist where isbooked= false";
                        using var cmdx = new NpgsqlCommand(adminid, connect);

                        using NpgsqlDataReader rdrs = cmdx.ExecuteReader();
                        while (rdrs.Read())
                        {
                            Console.WriteLine("{0} {1} {2} ", rdrs.GetString(0), rdrs.GetDate(1),
                             rdrs.GetTimeSpan(2));

                        }
                    }
                    else if (doctorlogin == "12349")
                    {
                        string adminid = "SELECT * FROM schedulepsychiatristsisbooked = false";
                        using var cmdx = new NpgsqlCommand(adminid, connect);

                        using NpgsqlDataReader rdrs = cmdx.ExecuteReader();
                        while (rdrs.Read())
                        {
                            Console.WriteLine("{0} {1} {2} ", rdrs.GetString(0), rdrs.GetDate(1),
                            rdrs.GetTimeSpan(2));

                        }
                    }




                }
                else if(doctorchoice == 2)
                {
                    // Book appointment with selected doctor
                    Console.WriteLine("Which date? (YYYYMMDD)");
                    Console.WriteLine("YYYY:");
                    string bookYear = Console.ReadLine();
                    Console.WriteLine("MM:");
                    string bookMonth = Console.ReadLine();
                    Console.WriteLine("DD");
                    string bookDay = Console.ReadLine();

                    DateOnly appointmantdate = new DateOnly(Int32.Parse(bookYear), Int32.Parse(bookMonth), Int32.Parse(bookDay));
                    Console.WriteLine("Which time? (HHMM)");
                    Console.WriteLine("HH");
                    string hour = Console.ReadLine();
                    Console.WriteLine("MM");
                    string minute = Console.ReadLine();
                    TimeOnly timeOnly = new TimeOnly(Int32.Parse(hour), Int32.Parse(minute));

                    Console.WriteLine("Available or not? (y/n)");
                    string confirm = Console.ReadLine();
                    bool changeConfirmed = false;

                    if (confirm == "y")
                    {
                        changeConfirmed = true;
                    }
                    else
                    {
                        changeConfirmed = false;
                    }
                    var css = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                    using var conn = new NpgsqlConnection(css);
                    conn.Open();

                    // call storedprocedures to update info
                    using var cmdd = new NpgsqlCommand("call bookappointment(@bookdate, @booktime, @confirmed)", conn);

                    cmdd.Parameters.AddWithValue("bookdate", appointmantdate);
                    cmdd.Parameters.AddWithValue("booktime", timeOnly);
                    cmdd.Parameters.AddWithValue("confirmed", changeConfirmed);

                    using NpgsqlDataReader rdrr = cmdd.ExecuteReader();

                }
                else if (doctorchoice == 3)
                {
                    var css = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                    using var conn = new NpgsqlConnection(css);
                    conn.Open();

                    using var cmdd = new NpgsqlCommand("SELECT * FROM scheduledentist where isbooked = true", conn);

                    Console.WriteLine("Here is all the upcoming appointments");

                    using NpgsqlDataReader rdrr = cmdd.ExecuteReader();

                    while (rdrr.Read())
                    {
                        Console.WriteLine("{0}  {1}  {2}      ", rdrr.GetString(0), rdrr.GetDate(1),
                              rdrr.GetTimeSpan(2));

                    }
                }
                else if (doctorchoice == 4)
                {
                    var css = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                    using var conn = new NpgsqlConnection(css);
                    conn.Open();

                    using var cmdd = new NpgsqlCommand("SELECT * FROM medicalappointments", conn);

                    Console.WriteLine("Here is all the medical appointments with diagnos and prescription for all the patient");

                    using NpgsqlDataReader rdrr = cmdd.ExecuteReader();

                    while (rdrr.Read())
                    {
                        Console.WriteLine("{0}  {1}  {2} {3} {4}      ", rdrr.GetInt32(0), rdrr.GetInt32(1), rdrr.GetDate(2),
                              rdrr.GetString(3), rdrr.GetString(4));

                    }
                }
                Console.WriteLine("--- DOCTOR MENU ---");
                Console.WriteLine("");
                Console.WriteLine("1. Availabilities");
                Console.WriteLine("2. Update Schedule");
                Console.WriteLine("3: See upcoming Schedule");
                Console.WriteLine("4: List Patient Medical Records");
                Console.WriteLine("0. Exit");

                doctorchoice = Convert.ToInt32(Console.ReadLine());
            }

            break;
        case 3:
            string loginAsAdmin = null;

            Console.WriteLine("Please type the password to login");
            string adminlogin = Console.ReadLine();
            while (adminlogin != connectAsAdmin(loginAsAdmin))
            {
                Console.WriteLine("Incorrect password please try again");
                adminlogin = Console.ReadLine();

            }

            Console.WriteLine("--- ADMIN MENU ---");
            Console.WriteLine("");
            Console.WriteLine("1. Add Doctor Specialization");
            Console.WriteLine("2. Add a new Doctor");
            Console.WriteLine("3: Delete a Doctor");
            Console.WriteLine("4: See list of patient");
            Console.WriteLine("5: See list of upcoming appointments");
            Console.WriteLine("6: See list of medical record");
            Console.WriteLine("7: See list of patient including medical number, fullname and total sum of all visit cost");
            Console.WriteLine("0. Exit");
            int admintChoice = Convert.ToInt32(Console.ReadLine());
            while (admintChoice != 0)
            {
                if (admintChoice == 1)
                {
                    //Add Doctor Specialization
                    Console.WriteLine("What specialization do you want to add?");
                    string specialization = Console.ReadLine();
                    Console.WriteLine("How much will it cost?");
                    int addVisitCost = int.Parse(Console.ReadLine());

                    var admin = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                    using var connect = new NpgsqlConnection(admin);
                    connect.Open();

                    string adminid = "Insert into doctorspecialization(spec, visitcost) VALUES(@spec, @visitcost)";
                    using var cmdx = new NpgsqlCommand(adminid, connect);
                    cmdx.Parameters.AddWithValue("spec", specialization);
                    cmdx.Parameters.AddWithValue("visitcost", addVisitCost);
                    using NpgsqlDataReader rdrr = cmdx.ExecuteReader();

                    Console.WriteLine("You have successfully added new specialization and visit cost");

                }
                else if(admintChoice == 2)
                {
                    var admin = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                    using var connect = new NpgsqlConnection(admin);
                    connect.Open();

                    Console.WriteLine("Register a new doctor to the database");
                    Console.WriteLine("Employee ID (5 digit)");
                    int employeeid = int.Parse(Console.ReadLine());
                    Console.WriteLine("Employee Full name");
                    string employeename = Console.ReadLine();
                    Console.WriteLine("Employee Specialization");
                    string spec = Console.ReadLine();
                    Console.WriteLine("The cost for visit");
                    int visitCost = int.Parse(Console.ReadLine());
                    Console.WriteLine("Employee Phonenumber");
                    int phoneNumber = int.Parse(Console.ReadLine());

                    string adminid = "Insert into doctor(employeeid, employeename, specialization, visitcost, phonenumber) VALUES(@employeeid, @employeename, @specialization, @visitcost, @phonenumber)";
                    using var cmdx = new NpgsqlCommand(adminid, connect);
                    cmdx.Parameters.AddWithValue("employeeid", employeeid);
                    cmdx.Parameters.AddWithValue("employeename", employeename);
                    cmdx.Parameters.AddWithValue("specialization", spec);
                    cmdx.Parameters.AddWithValue("visitcost", visitCost);
                    cmdx.Parameters.AddWithValue("phonenumber", phoneNumber);

                    using NpgsqlDataReader rdrr = cmdx.ExecuteReader();

                    Console.WriteLine("You have successfully added a new doctor to the database");
                }
                else if (admintChoice == 3)
                {
                    var admin = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                    using var connect = new NpgsqlConnection(admin);
                    connect.Open();

                    string adminid = "delete from public.doctor where doctor.employeeid = @doctorid";
                    using var cmdx = new NpgsqlCommand(adminid, connect);
                    Console.WriteLine("Type the employee id you want to delete");
                    int deleteEmployee = int.Parse(Console.ReadLine());
                    cmdx.Parameters.AddWithValue("doctorid", deleteEmployee);

                    using NpgsqlDataReader rdrr = cmdx.ExecuteReader();

                    Console.WriteLine("You have successfully deleted a doctor");
                }
                else if (admintChoice == 4)
                {
                    var css = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                    using var conn = new NpgsqlConnection(css);
                    conn.Open();



                    // call function getpatientbyid
                    //string patientinfo = "SELECT * FROM getpatientbyid (@test)";
                    using var cmdd = new NpgsqlCommand("SELECT * FROM patient", conn);



                    using NpgsqlDataReader rdrr = cmdd.ExecuteReader();

                    Console.WriteLine("MedicalID  FirstName  LastName  Gender  Adress     Phone     Birthdate  Registrationdate");
                    while (rdrr.Read())
                    {
                        Console.WriteLine("{0}  {1}      {2}     {3}    {4} {5} {6} {7}", rdrr.GetInt32(0), rdrr.GetString(1),
                              rdrr.GetString(2), rdrr.GetString(3), rdrr.GetString(4), rdrr.GetInt32(5), rdrr.GetDate(6), rdrr.GetDate(7));

                    }
                }
                else if (admintChoice == 5)
                {
                    var css = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                    using var conn = new NpgsqlConnection(css);
                    conn.Open();

                    using var cmdd = new NpgsqlCommand("SELECT * FROM scheduledentist where isbooked = true", conn);

                    Console.WriteLine("Here is all the upcoming appointments");

                    using NpgsqlDataReader rdrr = cmdd.ExecuteReader();

                    while (rdrr.Read())
                    {
                        Console.WriteLine("{0}  {1}  {2}      ", rdrr.GetString(0), rdrr.GetDate(1),
                              rdrr.GetTimeSpan(2));

                    }
                }
                else if (admintChoice == 6)
                {
                    var css = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                    using var conn = new NpgsqlConnection(css);
                    conn.Open();

                    using var cmdd = new NpgsqlCommand("SELECT * FROM medicalappointments", conn);

                    Console.WriteLine("Here is all the medical appointments with diagnos and prescription for all the patient");

                    using NpgsqlDataReader rdrr = cmdd.ExecuteReader();

                    while (rdrr.Read())
                    {
                        Console.WriteLine("{0}  {1}  {2} {3} {4}      ", rdrr.GetInt32(0), rdrr.GetInt32(1), rdrr.GetDate(2),
                              rdrr.GetString(3), rdrr.GetString(4));

                    }

                }
                else if (admintChoice == 7)
                {
                    var css = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

                    using var conn = new NpgsqlConnection(css);
                    conn.Open();

                    using var cmdd = new NpgsqlCommand("SELECT * FROM patientvisiting", conn);

                    Console.WriteLine("list of patient including medical number, fullname and total sum of all visit cost");

                    using NpgsqlDataReader rdrr = cmdd.ExecuteReader();

                    while (rdrr.Read())
                    {
                        Console.WriteLine("{0}  {1}  {2}     ", rdrr.GetInt32(0), rdrr.GetString(1), rdrr.GetInt32(2)
                              );

                    }
                }


                Console.WriteLine("--- ADMIN MENU ---");
                Console.WriteLine("");
                Console.WriteLine("1. Add Doctor Specialization");
                Console.WriteLine("2. Add a new Doctor");
                Console.WriteLine("3: Delete a Doctor");
                Console.WriteLine("4: See list of patient");
                Console.WriteLine("5: See list of upcoming appointments");
                Console.WriteLine("6: See list of medical record");
                Console.WriteLine("7: See list of patient including medical number, fullname and total sum of all visit cost");
                Console.WriteLine("0. Exit");
                admintChoice = Convert.ToInt32(Console.ReadLine());
            }
            break;
        case 0:
            Console.WriteLine("Closing application");
            break;


    }
    string connectAsAdmin(string storePassword)
    {
        var admin = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

        using var connect = new NpgsqlConnection(admin);
        connect.Open();

        string adminid = "SELECT * FROM public.admin";
        using var cmd = new NpgsqlCommand(adminid, connect);

        using NpgsqlDataReader rdr = cmd.ExecuteReader();


        while (rdr.Read())
        {
            //Console.WriteLine("{0} {1} {2} {3} {4} {5} {6} {7}", rdr.GetInt32(0), rdr.GetString(1),
            //      rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetInt32(5), rdr.GetDate(6), rdr.GetDate(7));


            storePassword = rdr.GetInt32(0).ToString();
            
        }
        return storePassword;
    }
    string[] connectAsDoctor(string[] storePassword)
    {
        var admin = "Host=pgserver.mau.se;Username=ak4368;Password=9ae50595;Database=ak4368";

        using var connect = new NpgsqlConnection(admin);
        connect.Open();

        string doctorid = "SELECT * FROM public.doctor";
        using var cmd = new NpgsqlCommand(doctorid, connect);

        using NpgsqlDataReader rdr = cmd.ExecuteReader();

        int n = 0;
        while (rdr.Read())
        {
            //Console.WriteLine("{0} {1} {2} {3} {4} {5} {6} {7}", rdr.GetInt32(0), rdr.GetString(1),
            //      rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetInt32(5), rdr.GetDate(6), rdr.GetDate(7));


            storePassword[n] = rdr.GetInt32(0).ToString();
            n++;
        }
        return storePassword;
    }

}
