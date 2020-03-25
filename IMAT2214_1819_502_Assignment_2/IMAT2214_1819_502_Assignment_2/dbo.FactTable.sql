CREATE TABLE [dbo].[FactTableAssignment]
(
	[productId]	INT	NOT NULL,
	[timeId] INT NOT NULL,
	[customerId] INT NOT NULL,
	[value] MONEY NOT NULL,
	[discount] FLOAT (53) NOT NULL,
	[profit] MONEY NOT NULL,
	[quantity] INT NOT NULL,
	CONSTRAINT [PK_FactTableAssignment] PRIMARY KEY ([timeId])
);
