import pandas as pd
import numpy as np
from sklearn.ensemble import RandomForestRegressor
from sklearn.impute import SimpleImputer

# Ask for the path of the CSV file
csv_path = input("Enter the path of the CSV file: ")

try:
    # Read the dataset
    df = pd.read_csv(csv_path)

    # Replace blank values with NaN
    df.replace('', np.nan, inplace=True)

    # Fill missing 'Z score' values based on district means
    district_means = df.groupby('District')['Z score'].transform('mean')
    df['Z score'].fillna(district_means, inplace=True)

    # Sort the data by year
    df.sort_values(by='Year', inplace=True)

    # Create lag variables for z-score values
    lag_periods = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]
    for lag in lag_periods:
        df[f'Z_score_lag_{lag}'] = df['Z score'].shift(lag)

    # Drop rows with NaN after lagging
    df.dropna(inplace=True)

    # Splitting data into features and target variable
    X = df[['Z_score_lag_1', 'Z_score_lag_2', 'Z_score_lag_3', 'Z_score_lag_4', 'Z_score_lag_5', 'Z_score_lag_6',
            'Z_score_lag_7', 'Z_score_lag_8', 'Z_score_lag_9', 'Z_score_lag_10', 'Z_score_lag_11', 'Z_score_lag_12']]
    y = df['Z score']

    # Impute missing values in data
    imputer = SimpleImputer(strategy='median')
    X_imputed = imputer.fit_transform(X)

    # Initialize Random Forest model
    rf_model = RandomForestRegressor(random_state=42)

    # Fit Random Forest model on the entire dataset
    rf_model.fit(X_imputed, y)

    # New year for prediction (modify this as needed)
    new_year_lag_values = [df['Z_score_lag_1'].iloc[-1], df['Z_score_lag_2'].iloc[-1], df['Z_score_lag_3'].iloc[-1],
                           df['Z_score_lag_4'].iloc[-1], df['Z_score_lag_5'].iloc[-1], df['Z_score_lag_6'].iloc[-1],
                           df['Z_score_lag_7'].iloc[-1], df['Z_score_lag_8'].iloc[-1], df['Z_score_lag_9'].iloc[-1],
                           df['Z_score_lag_10'].iloc[-1], df['Z_score_lag_11'].iloc[-1], df['Z_score_lag_12'].iloc[-1]]

    # Predict Z score for the new year
    new_year_z_score = rf_model.predict([new_year_lag_values])

    print(f"Predicted Z score for the new year: {new_year_z_score[0]}")

except Exception as e:
    print(f"Error: {e}. Please fix the dataset and try again.")
