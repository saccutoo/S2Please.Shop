UPDATE ML
		SET ML.LANGUAGE_ID=MLB.LANGUAGE_ID,
			ML.[KEY]=MLB.[KEY],
			ML.VALUE=MLB.VALUE
	FROM Shop.dbo.MULTI_LANGUAGE ML
	INNER JOIN ShopBak.dbo.MULTI_LANGUAGE MLB ON MLB.ID = ML.ID