<Query Kind="Program">
  <Output>DataGrids</Output>
</Query>

class City
{
	public int ID {get; set;}
	public string Name {get; set;}
	public string Code {get; set;}
	
	public City(int _id, string _name, string _code)
	{ 
		ID = _id; Name = _name; Code = _code; 
	}
}

class Order
{
	public int ID {get; set;}
	public int CustomerID {get; set;}
	public decimal Price {get; set;}
	public DateTime Date {get; set;}
	
	public Order(int _id, int _cust, decimal _price, DateTime _date)
	{ 
		ID = _id; CustomerID = _cust; Price = _price; Date = _date;
	}
}

class Customer
{
	public int ID {get; set;}
	public string Name {get; set;}
	public int CityID {get; set;}
	
	public Customer(int _id, string _name, int _city)
	{ 
		ID = _id; Name = _name; CityID = _city; 
	}
}

void Main()
{
	var customers = new List<Customer>();
	customers.Add(new Customer(1,"John Doe", 1));
	customers.Add(new Customer(2, "Jane Doe", 2));
	customers.Add(new Customer(3, "Mike Wock", 3));
	customers.Add(new Customer(4, "Rick Rotty", 1));
	customers.Add(new Customer(5, "Mannie Dowson", 2));
	customers.Add(new Customer(6, "Jacob Mann", 2));
	
	var orders = new List<Order>();
	orders.Add(new Order(1, 4, 10.00m, Convert.ToDateTime("2015-02-01")));
	orders.Add(new Order(2, 1, 12.00m, Convert.ToDateTime("2015-04-03")));
	orders.Add(new Order(3, 5, 7.50m, Convert.ToDateTime("2015-05-05")));
	orders.Add(new Order(4, 3, 15.50m, Convert.ToDateTime("2015-03-15")));
	orders.Add(new Order(5, 1, 29.00m, Convert.ToDateTime("2015-04-19")));
	orders.Add(new Order(6, 1, 12.00m, Convert.ToDateTime("2015-05-01")));
	orders.Add(new Order(7, 2, 18.50m, Convert.ToDateTime("2015-05-05")));
	orders.Add(new Order(8, 1, 7.00m, Convert.ToDateTime("2015-04-03")));
	orders.Add(new Order(9, 5, 16.00m, Convert.ToDateTime("2015-03-29")));
	orders.Add(new Order(10, 4, 10.00m, Convert.ToDateTime("2015-01-18")));
	orders.Add(new Order(11, 2, 8.00m, Convert.ToDateTime("2015-02-20")));
	orders.Add(new Order(12, 3, 10.00m, Convert.ToDateTime("2015-05-12")));
	orders.Add(new Order(13, 4, 10.00m, Convert.ToDateTime("2015-05-12")));
	
	var cities = new List<City>();
	cities.Add(new City(1, "Los Angeles", "323"));
	cities.Add(new City(2, "Dallas", "214"));
	cities.Add(new City(3, "New York", "212"));		
	
	//Console.WriteLine(orders);
	
//1. Клиентов, проживающих в Лос-Анджелесе.
	/*
	Console.WriteLine("1. Вывод клиентов, проживающих в Лос-Анджелесе.");
	var cust_cities = customers
		.Join(cities, cust => cust.CityID, cit => cit.ID, (cust, cit) => new 
		{
			id = cust.ID,
			name = cust.Name,
			city = cit.Name
		});
	//Console.WriteLine(cust_cities);
	
	var LA = 
		from cust in cust_cities
		where cust.city == "Los Angeles"
		select cust;
	Console.WriteLine("Клиенты, проживающие в Лос-Анджелесе");
	Console.WriteLine(LA);
	*/
//2. Число клиентов, не имеющих ни одного заказа.
	/*
	Console.WriteLine("2. Вывод числа клиентов, не имеющих ни одного заказа.");
	var cust_orders = 
		from ord in orders
		select ord.CustomerID;

	var cust_id = 
		from cust in customers
		select cust.ID;
		
	Console.WriteLine("Количество клиентов, не имеющих заказов: {0}", cust_id.Count() - cust_orders.Distinct().Count());
	Console.WriteLine("Количество клиентов, не имеющих заказов: {0}", cust_id.Except(cust_orders.Distinct()).Count());
	*/
//3. Для каждого клиента — имя клиента, город его проживания, код города, количество заказов, дату последнего заказа.
	/*
	Console.WriteLine("3. Вывод для каждого клиента — имя клиента, город его проживания, код города, количество заказов, дату последнего заказа");
	var cust_city_ord = customers
		.Join( cities, cust => cust.CityID, cit => cit.ID, ( cust, cit ) => new 
		{ 
			id = cust.ID, name = cust.Name, city = cit.Name, code = cit.Code,
			num_ord = orders.Where(o => o.CustomerID == cust.ID).Count(), 
			last_order = orders.Where(o => o.CustomerID == cust.ID).OrderByDescending(d => d.Date).Take(1).Select(d => d.Date)
		});
		
	
	Console.WriteLine(cust_city_ord);
	*/
	
	
	
//4. В алфавитном порядке тех клиентов, у которых количество заказов больше 2.
	/*
	Console.WriteLine("4. Вывод в алфавитном порядке тех клиентов, у которых количество заказов больше 2");
	
	var cust_ord = customers
		.Join( orders, cust => cust.ID, ord => ord.CustomerID, ( cust, cit ) => new 
		{ 
			name = cust.Name, num_ord = orders.Where(o => o.CustomerID == cust.ID).Count() > 2,
		});
	var cust_ord_2 = 
		from cust in cust_ord.Distinct().OrderBy(o => o.name)
		where cust.num_ord.Equals(true)
		select cust.name;
		
	Console.WriteLine(cust_ord_2);
	*/
//5. Сгруппированные по городу проживания клиенты, у которых есть заказы. Вывести для каждого имя и количество заказов.
	/*
	Console.WriteLine("5. Вывод cгруппированных по городу проживания клиенты, у которых есть заказы");
	var cust_ord = customers
		.Join( cities, cust => cust.CityID, cit => cit.ID, ( cust, cit ) => new 
		{ 
			name = cust.Name, city = cit.Name,
			num_ord = orders.Where(o => o.CustomerID == cust.ID).Count(),
		})
		.Where(o => o.num_ord > 0)
		.GroupBy(x => x.city);
	Console.WriteLine(cust_ord);
	*/
//6. Клиентов, у которых количество заказов меньше, чем среднее для клиентов из их города.
	/* Не победил :(
	Console.WriteLine("6. Вывод клиентов, у которых количество заказов меньше, чем среднее для клиентов из их города");
	var city_ord = cities
		.Join (customers, cit => cit.ID, cust => cust.CityID, ( cit, cust ) => new 
		//.Join (orders, cc => cc.cust.ID, ord => ord.CustomerID, (cc, ord) => new 
		{
			city = cit.Name, 
			num_cust = customers.Where(dd => dd.CityID == cit.ID).Count(),
			num_ord = orders.Where( o => o.CustomerID == cust.ID && cust.CityID == cit.ID).Count()
		})
		.GroupBy(d => new { d.num_cust, d.city, d.num_ord })
		.Select(g => new 
		{
			//id = g.Key.,
			city = g.Key.city,
			//num_ord = g.Key.num_ord,
			//num_cust = g.Key.num_cust,
			avg = g.Sum(x => x.num_ord) / g.Key.num_cust
			
		});
		
		//.GroupBy(x => x.CityID);
	Console.WriteLine(city_ord);	
	var cust_ord = customers
		.Join( cities, cust => cust.CityID, cit => cit.ID, ( cust, cit ) => new 
		{ 
			name = cust.Name, city = cit.Name,
			num_ord = orders.Where(o => o.CustomerID == cust.ID).Count(),
			city_avg = orders.Where(o => o.CustomerID == cust.ID).Count(),
		});
	Console.WriteLine(cust_ord);*/

//7. Город, в который поступило заказов на наибольшую сумму.
	/*
	Console.WriteLine("7. Вывод города, в который поступило заказов на наибольшую сумму");
	var city_cust_ord = cities
		.Join( customers, cit => cit.ID, cust => cust.CityID, ( cit, cust ) => new //{ cit, cust });
		{ 
			id = cit.ID, city = cit.Name, 
			sum_ord = orders.Where(o => o.CustomerID == cust.ID).Select(o => o.Price).Sum()
		})
		.GroupBy(d => new { d.id, d.city })
		.Select(g => new 
		{
			id = g.Key.id,
			city = g.Key.city,
			sum = g.Sum(x => x.sum_ord)
		})
		.OrderByDescending(x => x.sum)
		.Take(1)
		.Select(o => o.city);
	Console.WriteLine(city_cust_ord);
	Console.WriteLine(city_cust_ord.ElementAt(0));
	*/
//8. 3 клиентов, которые оформили заказов на наименьшую сумму либо не оформили заказов. 
//Вывести имя клиента, город, количество заказов и их общую сумму.
	/*
	Console.WriteLine("8. Вывод 3 клиентов, которые оформили заказов на наименьшую сумму либо не оформили заказов");
	var cust_ord = customers
		.Join( cities, cust => cust.CityID, cit => cit.ID, ( cust, cit ) => new 
		{ 
			name = cust.Name, city = cit.Name,
			num_ord = orders.Where(o => o.CustomerID == cust.ID).Count(),
			sum_ord = orders.Where(o => o.CustomerID == cust.ID).Select(o => o.Price).Sum()
		})
		.OrderBy(x => x.sum_ord)
		.Take(3);
		
	Console.WriteLine(cust_ord);
	*/
}