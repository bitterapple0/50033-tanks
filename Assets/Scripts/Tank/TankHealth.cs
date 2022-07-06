using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    public float m_StartingHealth = 100f;          
    public Slider m_Slider;                        
    public Image m_FillImage;                      
    public Color m_FullHealthColor = Color.green;  
    public Color m_ZeroHealthColor = Color.red;    
    public GameObject m_ExplosionPrefab;
    public FloatVariable m_Difficulty;
    public bool m_isAI = false;      
    public AudioSource m_HitAudio;          
    public AudioClip[] m_HitClips;      

    private AudioSource m_ExplosionAudio;
    private ParticleSystem m_ExplosionParticles;   
    private float m_CurrentHealth;  
    private bool m_Dead;
    

    private void Awake()
    {
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();
        m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();

        m_ExplosionParticles.gameObject.SetActive(false);

        m_HitAudio = GetComponents<AudioSource>()[1];
    }


    private void OnEnable()
    {
        m_CurrentHealth =  m_StartingHealth;
        m_Dead = false;

        SetHealthUI();
    }

    public void TakeDamage(float amount)
    {
        // Adjust the tank's current health, update the UI based on the new health and check whether or not the tank is dead.
        Debug.Log("Original " + amount);
        if(!m_isAI){
            amount += m_Difficulty.Value * 0.06f * amount; // Player takes more damage
            Debug.Log("Player took: " + amount);
        } else{
            amount -= m_Difficulty.Value * 0.15f * amount; // AI takes less damage
            Debug.Log("AI took: " + amount);
        }
        m_CurrentHealth -= amount ;
        SetRandomHitAudio();
        m_HitAudio.Play();
        SetHealthUI();
        if (m_CurrentHealth <= 0f && !m_Dead) OnDeath();
    }


    private void SetHealthUI()
    {
        // Adjust the value and colour of the slider.
 
        m_Slider.value = m_CurrentHealth;
        m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
    }

    private void SetRandomHitAudio()
    {
        int index = Random.Range(0,m_HitClips.Length);
        m_HitAudio.clip = m_HitClips[index];
    }


    private void OnDeath()
    {
        // Play the effects for the death of the tank and deactivate it.
        m_Dead = true;

        m_ExplosionParticles.transform.position = transform.position;
        m_ExplosionParticles.gameObject.SetActive(true);
        m_ExplosionParticles.Play();
        m_ExplosionAudio.Play();

        gameObject.SetActive(false);
    }
}