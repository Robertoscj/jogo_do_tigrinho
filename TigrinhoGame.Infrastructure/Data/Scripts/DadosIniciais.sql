USE TigrinhoGame;
GO

-- Insert Symbols
IF NOT EXISTS (SELECT * FROM Symbols)
BEGIN
    -- Símbolos regulares
    INSERT INTO Symbols (Name, Code, PayoutMultiplier3X, PayoutMultiplier4X, PayoutMultiplier5X, Weight, IsWild, IsScatter)
    VALUES 
        ('Tigrinho Dourado', 'TIGER', 10.0, 50.0, 500.0, 1, 0, 0),
        ('Peixe', 'FISH', 5.0, 20.0, 100.0, 2, 0, 0),
        ('Moeda', 'COIN', 3.0, 10.0, 50.0, 3, 0, 0),
        ('Dragão', 'DRAGON', 8.0, 40.0, 200.0, 2, 0, 0),
        ('Lanterna', 'LANTERN', 4.0, 15.0, 75.0, 3, 0, 0),
        ('Leque', 'FAN', 2.0, 8.0, 40.0, 4, 0, 0),
        ('Carta A', 'A', 1.5, 5.0, 25.0, 5, 0, 0),
        ('Carta K', 'K', 1.5, 5.0, 25.0, 5, 0, 0),
        ('Carta Q', 'Q', 1.0, 4.0, 20.0, 5, 0, 0),
        ('Carta J', 'J', 1.0, 4.0, 20.0, 5, 0, 0);

    -- Símbolos especiais
    INSERT INTO Symbols (Name, Code, PayoutMultiplier3X, PayoutMultiplier4X, PayoutMultiplier5X, Weight, IsWild, IsScatter)
    VALUES 
        ('Tigre Selvagem', 'WILD', 15.0, 75.0, 750.0, 1, 1, 0),
        ('Bônus', 'BONUS', 5.0, 20.0, 100.0, 1, 0, 1);
END
GO

-- Inserir linhas de pagamento
IF NOT EXISTS (SELECT * FROM PayLines)
BEGIN
    -- Linhas horizontais básicas
    INSERT INTO PayLines (Name, Positions)
    VALUES 
        ('Linha Superior', '0,0,0,0,0'),
        ('Linha Central', '1,1,1,1,1'),
        ('Linha Inferior', '2,2,2,2,2'),
        
        -- Linhas em forma de V
        ('V Shape Down', '0,1,2,1,0'),
        ('V Shape Up', '2,1,0,1,2'),
        
        -- Linhas do ziguezague
        ('Zigzag 1', '0,1,0,1,0'),
        ('Zigzag 2', '2,1,2,1,2'),
        
       -- Linhas em forma de W
        ('W Shape', '0,2,0,2,0'),
        ('M Shape', '2,0,2,0,2'),
        
        -- Linhas diagonais
        ('Diagonal Down', '0,0,1,2,2'),
        ('Diagonal Up', '2,2,1,0,0'),
        
        -- Padrões complexos
        ('Snake Down', '0,1,2,1,0'),
        ('Snake Up', '2,1,0,1,2'),
        ('Diamond', '1,0,1,0,1'),
        ('Reverse Diamond', '1,2,1,2,1');
END
GO 