#we will use R squared as it gives the best r squared error value. it isnt that good but with the thime and resources it is the best we can do.

import pandas as pd
import numpy as np
from sklearn.model_selection import train_test_split
from sklearn.linear_model import LinearRegression
from sklearn.ensemble import RandomForestRegressor, GradientBoostingRegressor
from sklearn.svm import SVR
from sklearn.neighbors import KNeighborsRegressor
from sklearn.impute import SimpleImputer
from sklearn.metrics import mean_squared_error, r2_score
from sklearn.preprocessing import LabelEncoder

# Read the dataset (if not already read)
df = pd.read_csv('/content/FinalProjectDataUCSC.csv')

# Replace blank values with NaN
df.replace('', np.nan, inplace=True)

# Fill missing 'Z score' values based on district means (if not already done)
district_means = df.groupby('District')['Z score'].transform('mean')
df['Z score'].fillna(district_means, inplace=True)

# Encode categorical variables (if not already done)
label_encoder = LabelEncoder()
df['University_encoded'] = label_encoder.fit_transform(df['University '])
df['Degree_type_encoded'] = label_encoder.fit_transform(df['Degree'])
df['District_encoded'] = label_encoder.fit_transform(df['District'])

# Sort the data by year (if not already done)
df.sort_values(by='Year', inplace=True)

# Create lag variables for z-score values (if not already done)
lag_periods = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]
for lag in lag_periods:
    df[f'Z_score_lag_{lag}'] = df['Z score'].shift(lag)

# Drop rows with NaN after lagging (if not already done)
df.dropna(inplace=True)

# Splitting data into features and target variable (if not already done)
X = df[['Z_score_lag_1', 'Z_score_lag_2', 'Z_score_lag_3', 'Z_score_lag_4', 'Z_score_lag_5', 'Z_score_lag_6',
        'Z_score_lag_7', 'Z_score_lag_8', 'Z_score_lag_9', 'Z_score_lag_10', 'Z_score_lag_11', 'Z_score_lag_12']]
y = df['Z score']

# Train-test split with a fixed seed for reproducibility
seed_value = 42  # You can change this value if needed
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=seed_value)

# Impute missing values in training data (if not already done)
imputer = SimpleImputer(strategy='median')
X_train_imputed = imputer.fit_transform(X_train)

# Initialize models (if not already done)
lr_model = LinearRegression()
rf_model = RandomForestRegressor(random_state=seed_value)
gb_model = GradientBoostingRegressor(random_state=seed_value)
svr_model = SVR()
knn_model = KNeighborsRegressor()

# Fit models (if not already done)
lr_model.fit(X_train_imputed, y_train)
rf_model.fit(X_train_imputed, y_train)
gb_model.fit(X_train_imputed, y_train)
svr_model.fit(X_train_imputed, y_train)
knn_model.fit(X_train_imputed, y_train)

# Predictions on test data
X_test_imputed = imputer.transform(X_test)
lr_predictions = lr_model.predict(X_test_imputed)
rf_predictions = rf_model.predict(X_test_imputed)
gb_predictions = gb_model.predict(X_test_imputed)
svr_predictions = svr_model.predict(X_test_imputed)
knn_predictions = knn_model.predict(X_test_imputed)

# Calculate R-squared for each model
lr_r_squared = r2_score(y_test, lr_predictions)
rf_r_squared = r2_score(y_test, rf_predictions)
gb_r_squared = r2_score(y_test, gb_predictions)
svr_r_squared = r2_score(y_test, svr_predictions)
knn_r_squared = r2_score(y_test, knn_predictions)

# Create a DataFrame to compare actual vs predicted values with the year and R-squared
comparison_df = pd.DataFrame({
    'Year': df.loc[X_test.index, 'Year'],  # Get the year from the original DataFrame for the test data
    'Actual': y_test.values,
    'Linear Regression Predicted': lr_predictions,
    'Random Forest Predicted': rf_predictions,
    'Gradient Boosting Predicted': gb_predictions,
    'SVR Predicted': svr_predictions,
    'KNN Predicted': knn_predictions,
    'Linear Regression R-squared': lr_r_squared,
    'Random Forest R-squared': rf_r_squared,
    'Gradient Boosting R-squared': gb_r_squared,
    'SVR R-squared': svr_r_squared,
    'KNN R-squared': knn_r_squared
})

print(comparison_df.head())
