using UnityEngine;

namespace Steering
{
    [System.Obsolete("Временное решение, заменить н SteeringManager")]
    [System.Serializable]
    public static class SteeringUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="velocity">Скорость агента</param>
        /// <param name="target">Положение цели</param>
        /// <param name="position">Позиция агента</param>
        /// <param name="maxVelocity">Максимальная скорость агента</param>
        /// <returns></returns>
        public static Vector2 Seek(Vector2 velocity, Vector2 target, Vector2 position, float maxVelocity = 1f)
        {
            Vector2 desired_velocity = maxVelocity * (target - position).normalized;
            Vector2 steering = desired_velocity - velocity;
            return steering;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="velocity">Скорость агента</param>
        /// <param name="target">Положение цели</param>
        /// <param name="position">Позиция агента</param>
        /// <param name="maxVelocity">Максимальная скорость агента</param>
        /// <returns></returns>
        public static Vector2 Flee(Vector2 velocity, Vector2 target, Vector2 position, float maxVelocity = 1f)
        {
            Vector2 desired_velocity = maxVelocity * (position - target).normalized;
            Vector2 steering = desired_velocity - velocity;
            return steering;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="velocity">Текущий вектор скорости агента</param>
        /// <param name="target">Положение цели</param>
        /// <param name="position">Положение агента</param>
        /// <param name="maxVelocity">Максимальная скорость агента</param>
        /// <param name="slowingRadius">Расстояние на котором происходит замедление</param>
        /// <returns></returns>
        public static Vector2 Arrive(Vector2 velocity, Vector2 target, Vector2 position, float maxVelocity = 2f, float slowingRadius = 2f, float stopRadius = 0.5f)
        {
            // Calculate the desired velocity
            Vector2 desired_velocity = target - position;
            float distance = desired_velocity.magnitude;

            // Check the distance to detect whether the character
            // is inside the slowing area
            if (distance < stopRadius)
            {
                desired_velocity = 0 * desired_velocity;
            }
            else if (distance < slowingRadius)
            {
                // Inside the slowing area
                desired_velocity = (desired_velocity.normalized) * maxVelocity * (distance / slowingRadius);
            }
            else
            {
                // Outside the slowing area.
                desired_velocity = (desired_velocity.normalized) * maxVelocity;
            }

            // Set the steering based on this
            Vector2 steering = desired_velocity - velocity;

            return steering;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="velocity">Текущая скорость агента</param>
        /// <param name="target">Положение цели</param>
        /// <param name="position">Положение агента</param>
        /// <param name="maxVelocity">Максимальная скорость</param>
        /// <param name="targetVelocity">Скорость цели</param>
        /// <returns></returns>
        public static Vector2 Pursue(Vector2 targetVelocity, Vector2 velocity, Vector2 target, Vector2 position, float maxVelocity = 1f)
        {
            float distance = (target - position).magnitude;
            float T = Time.deltaTime * distance / maxVelocity;
            Vector2 futurePosition = target + targetVelocity * T;
            Vector2 pursuit = Seek(velocity, target, position, maxVelocity);
            return pursuit;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="velocity">Текущая скорость агента</param>
        /// <param name="target">Положение цели</param>
        /// <param name="position">Положение агента</param>
        /// <param name="maxVelocity">Максимальная скорость</param>
        /// <param name="targetVelocity">Скорость цели</param>
        /// <returns></returns>
        public static Vector2 Evade(Vector2 targetVelocity, Vector2 velocity, Vector2 target, Vector2 position, float maxVelocity = 1f )
        {
            float distance = (target - position).magnitude;
            float T = Time.deltaTime * distance / maxVelocity;
            Vector2 futurePosition = target + targetVelocity * T;
            Vector2 evade = Flee(velocity, target, position, maxVelocity);
            return evade;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="velocity">Скорость агента</param>
        /// <param name="offset">Смещение</param>
        /// <param name="rate">Диапозн углов</param>
        /// <param name="agentOrientation">Ориентация агента</param>
        /// <param name="radius">Окружность соотвествующего радиуса где появится случайная точка</param>
        /// <returns></returns>
        public static Vector2 Wander(Vector2 velocity, float offset = 1f, float rate = 60f, float radius = 1f)
        {
            Vector2 circleCenter = offset * velocity.normalized;

            float agentOrientation = Vector2Angle(velocity);
            float wanderOrientation = Random.Range(-1.0f, 1.0f) * rate;
            float targetOrientation = wanderOrientation + agentOrientation;

            Vector2 displacement = radius * Angle2Vector(targetOrientation);
            Vector2 wander = displacement + circleCenter;


            return wander;
        }

        public static Vector2 Avoidance()
        {
            return new Vector2();
        }

        public static Vector2 Flocking(Vector2 position, Vector2[] velocity, Vector2[] positions, float AlignmentWeight = 1f, float CohesionWeigth = 1f, float SeperationWeight = 1f)
        {
            Vector2 alignment;
            Vector2 cohesion;
            Vector2 separation;
            int n;

            // Alignment rule
            n = velocity.Length;
            Vector2 averageDirection = new Vector2();
            for (int i = 0; i < n; i++)
            {
                averageDirection += velocity[i];
            }
            averageDirection /= n;
            alignment = averageDirection;//.normalized;

            //Cohesion rule
            n = positions.Length;
            Vector2 averagePosition = Vector2.zero;
            for (int i = 0; i < n; i++)
            {
                averagePosition += positions[i];
            }
            averagePosition /= n;
            cohesion = (averagePosition - position);//.normalized;

            //Separation rule
            n = positions.Length;
            Vector2 moveDirection = Vector2.zero;
            for (int i = 0; i < n; i++)
            {
                Vector2 v = positions[i] - position;
                moveDirection += v.normalized / (v.sqrMagnitude);
            }
            moveDirection /= n;
            separation = -moveDirection;//.normalized;

            ////Separation rule
            //n = positions.Length;
            //Vector2 moveDirection = Vector2.zero;
            //for (int i = 0; i < n; i++)
            //{
            //    moveDirection += positions[i] - position;
            //}
            //separation = -moveDirection / n;
            ////separation = separation.normalized;

            ////Separation rule
            //n = positions.Length;
            //Vector2 moveDirection = Vector2.zero;
            //for (int i = 0; i < n; i++)
            //{
            //    Vector2 v = positions[i] - position;
            //    moveDirection += v.normalized/(v.magnitude);
            //}
            ////separation = -moveDirection / n;
            //separation = -moveDirection.normalized;
            ////separation = -moveDirection;


            // Weighted sum
            Vector2 result =
                alignment.normalized * AlignmentWeight
                + cohesion.normalized * CohesionWeigth
                + separation.normalized * SeperationWeight;

            return result;
        }
        
        public static Vector2 Separation(Vector2 position, Vector2[] velocity, Vector2[] positions)
        {
            Vector2 separation;
            int n;

            //Separation rule
            n = positions.Length;
            Vector2 moveDirection = Vector2.zero;
            for (int i = 0; i < n; i++)
            {
                Vector2 v = positions[i] - position;
                moveDirection += v.normalized / v.sqrMagnitude;
            }
            if (n > 0) moveDirection /= n;
            //moveDirection = moveDirection.normalized;
            separation = -moveDirection;
            
            return separation;
        }

        public static Vector2 FollowPath()
        {
            return new Vector2();
        }

        public static Vector2 SimplePotentialField(Vector2 position, Vector2 goal, Vector2[] obstacles, float[] forces)
        {
            float distance2;
            Vector2 f;
            Vector2 F = new Vector2();
            int n = obstacles.Length;
            for (int i = 0; i < n; i++)
            {
                f = position - obstacles[i];
                distance2 = f.sqrMagnitude;
                F += f.normalized/distance2;
            }
            F += (goal - position).normalized;
            return F;
        }
        
        public static Vector2 Angle2Vector(float alpha)
        {
            Vector2 vector = Vector2.zero;
            float radius = 1.0f;
            if (alpha < 0) alpha += 2 * Mathf.PI;

            vector.y = Mathf.Sin(alpha * Mathf.Deg2Rad) * radius;
            vector.x = Mathf.Cos(alpha * Mathf.Deg2Rad) * radius;

            return vector.normalized;
        }

        public static float Vector2Angle(Vector2 direction)
        {
            float alpha = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (alpha < 0) alpha += 2 * Mathf.PI;
            return alpha;
        }
    }
}
