drop table BookUserMood;
drop table Search;
drop table DBUser;
drop table Book;
drop table Mood;

create table DBUser (
	userID int not null auto_increment, 
	username varchar(255) not null, 
	email varchar(255) not null, 
	password varchar(255) not null, 
	first_name varchar(255) not null, 
	last_name varchar(255) not null, 
	primary key (userID)
);

create table Book (
	bookID int not null auto_increment, 
	isbn varchar(255) not null, 
	title varchar(255) not null, 
	author varchar(255) not null,
	imageURL varchar(255) not null,
  downloadURL varchar(255) not null,
	publishedDate Date not null, 
	uploadDate Date not null, 
	primary key (bookID)
);

create table Mood(
	moodID int not null auto_increment,
	name varchar(255) not null,
	primary key (moodID)
);

create table Search(
	searchID int not null auto_increment,
	bookID int not null, 
	userID int not null,
	searchDate Date not null, 
	primary key (searchID), 
	foreign key (bookID) references Book(bookID), 
	foreign key (userID) references DBUser(userID)
);
create table BookUserMood(
	bookID int not null,
	userID int not null,
	moodID int not null,
	primary key (bookID, userID),
	foreign key (bookID) references Book(bookID),
	foreign key (userID) references DBUser(userID),
	foreign key (moodID) references Mood(moodID)
);

insert into DBUser (username, email, password, first_name, last_name) values ('alexRoma87', 'alessio.iannella@gmail.com', 'alessio', 'Alessio', 'Iannella');
insert into DBUser (username, email, password, first_name, last_name) values ('cindyBasha87', 'cindy.basha@gmail.com', 'cindy', 'Cindy', 'Basha');
insert into DBUser (username, email, password, first_name, last_name) values ('marcoRoma90', 'marco.iannella@gmail.com', 'marco', 'Marco', 'Iannella');

insert into Book (isbn, title, author, imageURL, downloadURL, publishedDate, uploadDate) values ('ASBNSDFSFGSH1234', 'A scary mansion', 'Mark Kain', 'https://storage.googleapis.com/api_project_books/book.png', 'https://storage.googleapis.com/api_project_books/oauth.pdf', '20120618' , '20171219');
insert into Book (isbn, title, author, imageURL, downloadURL, publishedDate, uploadDate) values ('ASBNSDFSDFSH1234', 'The new Soccer', 'Alfred Gota', 'https://storage.googleapis.com/api_project_books/book.png', 'https://storage.googleapis.com/api_project_books/oauth.pdf', '20130618' , '20171219');
insert into Book (isbn, title, author, imageURL, downloadURL, publishedDate, uploadDate) values ('ASBNSDFSERSH1234', 'Dad I found mom', 'Theresa Dill', 'https://storage.googleapis.com/api_project_books/book.png', 'https://storage.googleapis.com/api_project_books/oauth.pdf', '20140618' , '20171219');
insert into Book (isbn, title, author, imageURL, downloadURL, publishedDate, uploadDate) values ('ASBNSDFSTYSH1234', 'The unbreakable', 'Mark Twain', 'https://storage.googleapis.com/api_project_books/book.png', 'https://storage.googleapis.com/api_project_books/oauth.pdf', '20150618' , '20171219');
insert into Book (isbn, title, author, imageURL, downloadURL, publishedDate, uploadDate) values ('ASBNSGHSDFSH1234', 'Every single day', 'Moe Gloss', 'https://storage.googleapis.com/api_project_books/book.png', 'https://storage.googleapis.com/api_project_books/oauth.pdf', '20160618' , '20171219');
insert into Book (isbn, title, author, imageURL, downloadURL, publishedDate, uploadDate) values ('ASBNSDFSDFSH1234', 'The dark inside me', 'Rey Winderlich', 'https://storage.googleapis.com/api_project_books/book.png', 'https://storage.googleapis.com/api_project_books/oauth.pdf', '20170618' , '20171219');
insert into Book (isbn, title, author, imageURL, downloadURL, publishedDate, uploadDate) values ('ASBNSDFJKFSH1234', 'Happy birthday', 'Todd Fox', 'https://storage.googleapis.com/api_project_books/book.png', 'https://storage.googleapis.com/api_project_books/oauth.pdf', '20120518' , '20171219');
insert into Book (isbn, title, author, imageURL, downloadURL, publishedDate, uploadDate) values ('ASBNSDFSECSH1234', 'Sweet home', 'Yun Zei', 'https://storage.googleapis.com/api_project_books/book.png', 'https://storage.googleapis.com/api_project_books/oauth.pdf', '20120418' , '20171219');
insert into Book (isbn, title, author, imageURL, downloadURL, publishedDate, uploadDate) values ('ASBNSDFSEXSH1234', '1989: last run', 'Vincent Gohar', 'https://storage.googleapis.com/api_project_books/book.png', 'https://storage.googleapis.com/api_project_books/oauth.pdf', '20120318' , '20171219');
insert into Book (isbn, title, author, imageURL, downloadURL, publishedDate, uploadDate) values ('ASBWEDFSDFSH1234', 'The world war II', 'Eliza Monetti', 'https://storage.googleapis.com/api_project_books/book.png', 'https://storage.googleapis.com/api_project_books/oauth.pdf', '20120218' , '20171219');
insert into Book (isbn, title, author, imageURL, downloadURL, publishedDate, uploadDate) values ('ASBNSDFSDFSE3234', 'Ceasar the emperor', 'Robert Baratheon', 'https://storage.googleapis.com/api_project_books/book.png', 'https://storage.googleapis.com/api_project_books/oauth.pdf', '20120118' , '20171219');
insert into Book (isbn, title, author, imageURL, downloadURL, publishedDate, uploadDate) values ('ASBHYDFSDFSH1234', 'Rome: a new empire', 'Eder Leis', 'https://storage.googleapis.com/api_project_books/book.png', 'https://storage.googleapis.com/api_project_books/oauth.pdf', '20120618' , '20171219');
insert into Book (isbn, title, author, imageURL, downloadURL, publishedDate, uploadDate) values ('ASBNSDASDFSH1234', 'The future tech', 'Sarah Coyle', 'https://storage.googleapis.com/api_project_books/book.png', 'https://storage.googleapis.com/api_project_books/oauth.pdf', '20120718' , '20171219');
insert into Book (isbn, title, author, imageURL, downloadURL, publishedDate, uploadDate) values ('ASBNSDDFDFSH1234', 'The red balloon', 'Anna Red', 'https://storage.googleapis.com/api_project_books/book.png', 'https://storage.googleapis.com/api_project_books/oauth.pdf', '20120818' , '20171219');
insert into Book (isbn, title, author, imageURL, downloadURL, publishedDate, uploadDate) values ('ASBNSDVBDFSH1234', 'When it is too late', 'Simon Mit', 'https://storage.googleapis.com/api_project_books/book.png', 'https://storage.googleapis.com/api_project_books/oauth.pdf', '20120918' , '20171219');
insert into Book (isbn, title, author, imageURL, downloadURL, publishedDate, uploadDate) values ('ASBNSDNMDFSH1234', 'My old friend', 'Alex Say', 'https://storage.googleapis.com/api_project_books/book.png', 'https://storage.googleapis.com/api_project_books/oauth.pdf', '20121018', '20171219');
insert into Book (isbn, title, author, imageURL, downloadURL, publishedDate, uploadDate) values ('ASBNSDZXDFSH1234', 'The lord of the rings', 'J. R. R. Tolkien', 'https://storage.googleapis.com/api_project_books/book.png', 'https://storage.googleapis.com/api_project_books/oauth.pdf', '20121118' , '20171219');
insert into Book (isbn, title, author, imageURL, downloadURL, publishedDate, uploadDate) values ('ASBNSDVBDFSH1234', 'The hobbit', 'J. R. R. Tolkien', 'https://storage.googleapis.com/api_project_books/book.png', 'https://storage.googleapis.com/api_project_books/oauth.pdf', '20121218' , '20171219');
insert into Book (isbn, title, author, imageURL, downloadURL, publishedDate, uploadDate) values ('ASBNSZXCDFSH1234', 'Adventure time', 'Josh Kor', 'https://storage.googleapis.com/api_project_books/book.png', 'https://storage.googleapis.com/api_project_books/oauth.pdf', '20120718' , '20171219');
insert into Book (isbn, title, author, imageURL, downloadURL, publishedDate, uploadDate) values ('ASBNSASEDFSH1234', 'IT', 'Stephen King', 'https://storage.googleapis.com/api_project_books/book.png', 'https://storage.googleapis.com/api_project_books/oauth.pdf', '20120624' , '20171219');

insert into Mood (name) values ('Relax');
insert into Mood (name) values ('Party');
insert into Mood (name) values ('Fear');
insert into Mood (name) values ('Future');
insert into Mood (name) values ('Adventure');
insert into Mood (name) values ('Laugh');
insert into Mood (name) values ('Euphoria');
insert into Mood (name) values ('Love');
insert into Mood (name) values ('Dark');
insert into Mood (name) values ('Hope');

insert into BookUserMood values (1,1,1);
insert into BookUserMood values (2,1,2);
insert into BookUserMood values (3,1,2);
insert into BookUserMood values (4,1,2);
insert into BookUserMood values (5,1,3);
insert into BookUserMood values (6,2,1);
insert into BookUserMood values (7,2,1);
insert into BookUserMood values (8,2,4);
insert into BookUserMood values (9,2,4);
insert into BookUserMood values (10,2,5);
insert into BookUserMood values (11,2,5);
insert into BookUserMood values (12,3,5);
insert into BookUserMood values (13,3,6);
insert into BookUserMood values (14,3,6);
insert into BookUserMood values (15,3,7);
insert into BookUserMood values (16,3,7);
insert into BookUserMood values (17,3,1);
insert into BookUserMood values (18,3,1);
insert into BookUserMood values (1,3,2);
insert into BookUserMood values (2,3,2);

insert into Search (bookID, userID, searchDate) values (1, 1, '20171220');
insert into Search (bookID, userID, searchDate) values (2, 1, '20171220');
insert into Search (bookID, userID, searchDate) values (3, 1, '20171220');
insert into Search (bookID, userID, searchDate) values (4, 1, '20171220');
insert into Search (bookID, userID, searchDate) values (5, 1, '20171220');
insert into Search (bookID, userID, searchDate) values (6, 1, '20171220');
insert into Search (bookID, userID, searchDate) values (7, 1, '20171220');
insert into Search (bookID, userID, searchDate) values (8, 1, '20171220');
insert into Search (bookID, userID, searchDate) values (9, 1, '20171220');
insert into Search (bookID, userID, searchDate) values (10, 1, '20171220');
insert into Search (bookID, userID, searchDate) values (11, 2, '20171220');
insert into Search (bookID, userID, searchDate) values (12, 2, '20171220');
insert into Search (bookID, userID, searchDate) values (13, 2, '20171220');
insert into Search (bookID, userID, searchDate) values (14, 2, '20171220');
insert into Search (bookID, userID, searchDate) values (15, 2, '20171220');
insert into Search (bookID, userID, searchDate) values (16, 2, '20171220');
insert into Search (bookID, userID, searchDate) values (17, 2, '20171220');
insert into Search (bookID, userID, searchDate) values (18, 2, '20171220');
insert into Search (bookID, userID, searchDate) values (19, 2, '20171220');
insert into Search (bookID, userID, searchDate) values (1, 2, '20171220');
insert into Search (bookID, userID, searchDate) values (3, 3, '20171220');
insert into Search (bookID, userID, searchDate) values (5, 3, '20171220');
insert into Search (bookID, userID, searchDate) values (7, 3, '20171220');
insert into Search (bookID, userID, searchDate) values (9, 3, '20171220');
insert into Search (bookID, userID, searchDate) values (11, 3, '20171220');
insert into Search (bookID, userID, searchDate) values (13, 3, '20171220');
insert into Search (bookID, userID, searchDate) values (15, 3, '20171220');
insert into Search (bookID, userID, searchDate) values (17, 3, '20171220');
insert into Search (bookID, userID, searchDate) values (19, 3, '20171220');
insert into Search (bookID, userID, searchDate) values (2, 3, '20171220');

